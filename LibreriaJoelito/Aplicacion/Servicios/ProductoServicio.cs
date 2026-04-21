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
            if (idPresentacion <= 0) return Result<int>.Failure("Debe seleccionar una presentación válida.");
            if (factorConversion <= 0) return Result<int>.Failure("El factor de conversión debe ser mayor a cero.");
            if (precioVenta <= 0) return Result<int>.Failure("El precio de venta debe ser mayor a cero.");

            using (var scope = new TransactionScope())
            {
                try
                {
                    int nuevoIdProducto = productoRepository.Insert(producto);
                    if (nuevoIdProducto <= 0) throw new Exception("Error al insertar el producto principal.");

                    int relacionExitosa = presentacionProductoRepository.InsertarRelacion(
                        nuevoIdProducto, idPresentacion, factorConversion, precioVenta, producto.IdUsuario ?? 1);

                    if (relacionExitosa <= 0) throw new Exception("Error al asociar la presentación y el precio.");

                    scope.Complete();
                    return Result<int>.Success(nuevoIdProducto);
                }
                catch (Exception ex)
                {
                    return Result<int>.Failure($"Error en transacción: {ex.Message}");
                }
            }
        }

        // ---> EL NUEVO MÉTODO PARA AGREGAR PRESENTACIONES <---
        public Result AsociarNuevaPresentacion(int idProducto, int idPresentacion, int factor, decimal precio, int idUsuario)
        {
            try
            {
                if (idProducto <= 0) return Result.Failure("Producto no válido.");
                if (idPresentacion <= 0) return Result.Failure("Debe seleccionar una presentación.");
                if (factor <= 0) return Result.Failure("El factor debe ser mayor a cero.");
                if (precio <= 0) return Result.Failure("El precio debe ser mayor a cero.");

                // Verificamos si ya existe para evitar errores SQL de llave duplicada
                var existente = presentacionProductoRepository.GetByIds(idProducto, idPresentacion);
                if (existente != null) return Result.Failure("Este producto ya tiene registrada esta presentación.");

                int filas = presentacionProductoRepository.InsertarRelacion(idProducto, idPresentacion, factor, precio, idUsuario);

                return filas > 0 ? Result.Success() : Result.Failure("No se pudo registrar la presentación.");
            }
            catch (Exception ex)
            {
                return Result.Failure("Error de base de datos: " + ex.Message);
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