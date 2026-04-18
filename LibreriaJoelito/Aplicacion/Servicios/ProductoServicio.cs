using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Results;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;
using MySqlX.XDevAPI;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Servicios
{
    public class ProductoServicio
    {
        private readonly IProductoRepository productoRepository;
        private readonly ProductValidator productoValidator;

        public ProductoServicio(IProductoRepository productoRepository, ProductValidator productoValidator)
        {
            this.productoRepository = productoRepository;
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

        public Result Insert(Producto producto)
        {
            var validationResults = productoValidator.ValidarProducto(producto);

            if (validationResults.Any())
            {
                var errors = validationResults
                    .Select(v =>
                    {
                        var field = v.MemberNames.FirstOrDefault() ?? "General";
                        return $"{field}: {v.ErrorMessage}";
                    })
                    .ToList();

                return Result.Failure(errors);
            }

            productoRepository.Insert(producto);

            return Result.Success();
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
