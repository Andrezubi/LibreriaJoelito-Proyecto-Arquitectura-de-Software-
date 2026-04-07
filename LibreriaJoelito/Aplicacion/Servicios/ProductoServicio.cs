using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Results;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using MySqlX.XDevAPI;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Servicios
{
    public class ProductoServicio
    {
        private readonly IRepository<Producto> productoRepository;
        private readonly ProductValidator productoValidator;

        public ProductoServicio(IRepository<Producto> clienteRepository, ProductValidator productoValidator)
        {
            this.productoRepository = clienteRepository;
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
    }
}
