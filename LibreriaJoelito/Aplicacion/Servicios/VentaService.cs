using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Results;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibreriaJoelito.Aplicacion.Servicios
{
    public class VentaService 
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IDetalleVentaRepository _detalleVentaRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IPresentacionProductoRepository _presentaProdRepository;
        private readonly IPdfService _pdfService;
        public VentaService(
            IPresentacionProductoRepository presentProdRepository,
            IVentaRepository ventaRepository,
            IDetalleVentaRepository detalleVentaRepository,
            IProductoRepository productoRepository,
            IPdfService pdfService,
            IClienteRepository clienteRepository)
        {
            _ventaRepository = ventaRepository;
            _detalleVentaRepository = detalleVentaRepository;
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
            _presentaProdRepository = presentProdRepository;
            _pdfService = pdfService;
        }
        public DataTable getPresentacionProductosByFrase(string frase)
        {
            return _presentaProdRepository.obtenerPresentacionProductoDetallado(frase);
        }
        public Result<int> RegistrarVenta(Venta venta, List<DetalleVenta> detalles)
        {
            try
            {
                // 1. Validaciones previas (Fuera de transacción para no bloquear)
                if (detalles == null || !detalles.Any())
                    return Result<int>.Failure("La venta debe tener al menos un producto.");

                var clienteRow = _clienteRepository.GetById(venta.IdCliente);
                if (clienteRow == null)
                    return Result<int>.Failure("El cliente seleccionado no es válido.");

                // 2. Iniciar Proceso Atómico
                RepositorioBD.Instancia.BeginTransaction();

                try
                {
                    // 3. Insertar Cabecera de Venta
                    int ventaId = _ventaRepository.Insert(venta);
                    if (ventaId <= 0)
                        throw new Exception("No se pudo generar la cabecera de la venta.");

                    // 4. Procesar Detalles y Stock
                    foreach (var detalle in detalles)
                    {
                        detalle.IdVenta = ventaId;

                        // Insertar Detalle
                        int filasDetalle = _detalleVentaRepository.Insert(detalle);
                        if (filasDetalle <= 0)
                            throw new Exception($"Error al insertar el detalle para el producto: {_productoRepository.GetById(detalle.IdProducto)?["Nombre"]}");

                        // Descontar Stock y validar suficiencia
                        int filasStock = _productoRepository.DescontarStock(detalle.IdProducto, detalle.Cantidad);
                        if (filasStock <= 0)
                        {
                            // Si no afectó filas es porque el Stock < Cantidad (validación lógica en el SQL)
                            throw new Exception($"Stock insuficiente para el producto: {_productoRepository.GetById(detalle.IdProducto)?["Nombre"]}");
                        }
                    }

                    // 5. Confirmar todo
                    RepositorioBD.Instancia.Commit();
                    return Result<int>.Success(ventaId);
                }
                catch (Exception ex)
                {
                    // 6. Revertir si algo falló
                    RepositorioBD.Instancia.Rollback();
                    return Result<int>.Failure($"Error en la transacción: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error inesperado: {ex.Message}");
            }
        }

        public Result<int> AnularVenta(int idVenta)
        {
            try
            {
                var ventaRow = _ventaRepository.GetById(idVenta);
                if (ventaRow == null)
                    return Result<int>.Failure("La venta ya ha sido anulada antes.");

                RepositorioBD.Instancia.BeginTransaction();

                try
                {
                    DataTable detallesDt = _detalleVentaRepository.GetByIdVenta(Convert.ToInt32(idVenta));

                    foreach (DataRow row in detallesDt.Rows)
                    {
                        int idProducto = Convert.ToInt32(row["IdProducto"]);
                        int cantidad = Convert.ToInt32(row["Cantidad"]) * Convert.ToInt32(row["FactorConversion"]);

                        int filasStock = _productoRepository.RestaurarStock(idProducto, cantidad);
                        if (filasStock <= 0)
                            return Result<int>.Failure($"Error al restaurar el stock del producto ID {idProducto}.");
                    }

                    Venta venta = new Venta
                    {
                        Id = idVenta,
                        IdUsuario = Convert.ToInt32(ventaRow["IdUsuario"])
                    };

                    int resultado = _ventaRepository.Delete(venta);
                    if (resultado <= 0)
                        return Result<int>.Failure("No se pudo actualizar el estado de la venta.");

                    RepositorioBD.Instancia.Commit();
                    return Result<int>.Success(venta.Id);
                }
                catch (Exception ex)
                {
                    RepositorioBD.Instancia.Rollback();
                    return Result<int>.Failure($"Transacción revertida. Error: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Error inesperado al anular: {ex.Message}");
            }
        }

        public JsonResult getPresentacionProductoByIds(int idProducto, int idPresentacion)
        {
            DataRow row = _presentaProdRepository.GetByIds(idProducto, idPresentacion);

            if (row != null)
            {
                return new JsonResult(new
                {
                    success = true,
                    producto = new
                    {
                        idProducto = idProducto,
                        idPresentacion=idPresentacion,
                        nombre = row["Descripcion"].ToString(),
                        precioUnitario = Convert.ToDecimal(row["Precio"])
                    }
                });
            }

            return new JsonResult(new { success = false });
        }

        public Result<byte[]> GenerarComprobantePdf(int idVenta)
        {
            try
            {
                // 1. Pedimos los datos al repositorio (La consulta de los Joins)
                DataTable dt = _ventaRepository.ObtenerDatosComprobante(idVenta);

                if (dt == null || dt.Rows.Count == 0)
                    return Result<byte[]>.Failure("No se encontró la venta.");

                // 2. Delegamos la creación del archivo al servicio especializado
                byte[] pdf = _pdfService.GenerarComprobanteVenta(dt);

                return Result<byte[]>.Success(pdf);
            }
            catch (Exception ex)
            {
                return Result<byte[]>.Failure($"Error en fachada de PDF: {ex.Message}");
            }
        }
    }
}
