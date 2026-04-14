using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Results;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia;

namespace LibreriaJoelito.Aplicacion.Servicios
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepository;
        private readonly IDetalleVentaRepository _detalleVentaRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly IClienteRepository _clienteRepository;

        public VentaService(
            IVentaRepository ventaRepository,
            IDetalleVentaRepository detalleVentaRepository,
            IProductoRepository productoRepository,
            IClienteRepository clienteRepository)
        {
            _ventaRepository = ventaRepository;
            _detalleVentaRepository = detalleVentaRepository;
            _productoRepository = productoRepository;
            _clienteRepository = clienteRepository;
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
                            throw new Exception($"Error al insertar el detalle para el producto ID: {detalle.IdProducto}");

                        // Descontar Stock y validar suficiencia
                        int filasStock = _productoRepository.DescontarStock(detalle.IdProducto, detalle.Cantidad);
                        if (filasStock <= 0)
                        {
                            // Si no afectó filas es porque el Stock < Cantidad (validación lógica en el SQL)
                            throw new Exception($"Stock insuficiente para el producto ID: {detalle.IdProducto}");
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

        public Result AnularVenta(int idVenta)
        {
            try
            {
                var ventaRow = _ventaRepository.GetById(idVenta);
                if (ventaRow == null)
                    return Result.Failure("La venta no existe.");

                Venta venta = new Venta
                {
                    Id = idVenta,
                    IdUsuario = Convert.ToInt32(ventaRow["IdUsuario"]) // O el usuario actual de la sesión
                };

                int resultado = _ventaRepository.Delete(venta);
                
                if (resultado > 0)
                    return Result.Success();
                
                return Result.Failure("No se pudo anular la venta.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Error al anular: {ex.Message}");
            }
        }
    }
}
