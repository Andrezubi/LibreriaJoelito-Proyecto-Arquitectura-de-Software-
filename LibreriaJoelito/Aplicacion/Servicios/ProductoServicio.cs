using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Results;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Transactions;

namespace LibreriaJoelito.Aplicacion.Servicios
{
    public class ProductoServicio
    {
        private readonly IProductoRepository productoRepository;
        private readonly IPresentacionProductoRepository presentacionProductoRepository;
        private readonly ProductValidator productoValidator;

        public ProductoServicio(
            IProductoRepository productoRepository,
            IPresentacionProductoRepository presentacionProductoRepository,
            ProductValidator productoValidator)
        {
            this.productoRepository = productoRepository;
            this.presentacionProductoRepository = presentacionProductoRepository;
            this.productoValidator = productoValidator;
        }

        public DataTable GetAll()
        {
            return productoRepository.GetAll();
        }

        public DataRow GetById(int id)
        {
            return productoRepository.GetById(id);
        }

        public Result<int> Insert(Producto producto, int idPresentacion, int factorConversion, decimal precioVenta)
        {
            // Validaciones de negocio preventivas
            if (idPresentacion <= 0) return Result<int>.Failure("Debe seleccionar una presentación válida.");
            if (factorConversion <= 0) return Result<int>.Failure("El factor de conversión debe ser mayor a cero.");
            if (precioVenta <= 0) return Result<int>.Failure("El precio de venta debe ser mayor a cero.");

            // Usamos TransactionScope para que ambas inserciones sean "Todo o Nada"
            using (var scope = new TransactionScope())
            {
                try
                {
                    // 1. Insertamos el producto principal y recuperamos el ID generado
                    int nuevoIdProducto = productoRepository.Insert(producto);
                    if (nuevoIdProducto <= 0) throw new Exception("Error al insertar el producto principal.");

                    // 2. Insertamos la relación (Producto + Presentación + Precio)
                    // El método en tu Repo espera factorConversion como double, por lo que int pasa sin problema
                    int relacionExitosa = presentacionProductoRepository.InsertarRelacion(
                        nuevoIdProducto, idPresentacion, factorConversion, precioVenta, producto.IdUsuario);

                    if (relacionExitosa <= 0) throw new Exception("Error al asociar la presentación y el precio.");

                    // 3. Confirmamos la transacción
                    scope.Complete();
                    return Result<int>.Success(nuevoIdProducto);
                }
                catch (Exception ex)
                {
                    return Result<int>.Failure($"Error en transacción: {ex.Message}");
                }
            }
        }

        public Result Update(Producto producto)
        {
            var validationResults = productoValidator.ValidarProducto(producto);

            if (validationResults.Any())
            {
                var errors = validationResults
                    .Select(v =>
                    {
                        var field = v.MemberNames.FirstOrDefault() ?? "General";
                        return $"{v.ErrorMessage}";
                    })
                    .ToList();

                return Result.Failure(errors);
            }

            productoRepository.Update(producto);

            return Result.Success();
        }

        public int Delete(Producto producto)
        {
            return productoRepository.Delete(producto);
        }

        public DataTable BuscarPorNombre(string frase)
        {
            return productoRepository.BuscarPorNombre(frase);
        }

        public DataTable BuscarProducto(string nombre)
        {
            return productoRepository.BuscarProducto(nombre);
        }
    }
}