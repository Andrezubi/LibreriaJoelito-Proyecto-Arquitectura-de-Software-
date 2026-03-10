using LibreriaJoelito.Models;
using MySqlX.XDevAPI;
using System.ComponentModel.DataAnnotations;

namespace LibreriaJoelito.Validators
{
    public static class ProductValidator
    {
        public static List<ValidationResult> ValidarProducto(Producto producto)
        {
            var errores = new List<ValidationResult>();

            ValidarNombre(producto.Nombre, errores);
            ValidarCategoria(producto.Categoria, errores);
            ValidarPrecio(producto.Precio, errores);
            ValidarStock(producto.Stock, errores);
            ValidarTipoVenta(producto.TipoVenta, errores);
            ValidarFactorConversion(producto.FactorConversion, errores);
            validarIdProductoBase(producto.IdProductoBase, errores);

            return errores;
        }

        private static void validarIdProductoBase(int? idProductoBase, List<ValidationResult> errores)
        {
            throw new NotImplementedException();
        }

        private static void ValidarFactorConversion(decimal? factorConversion, List<ValidationResult> errores)
        {
            throw new NotImplementedException();
        }

        private static void ValidarTipoVenta(string tipoVenta, List<ValidationResult> errores)
        {
            throw new NotImplementedException();
        }

        private static void ValidarStock(int stock, List<ValidationResult> errores)
        {
            throw new NotImplementedException();
        }

        private static void ValidarPrecio(decimal precio, List<ValidationResult> errores)
        {
            throw new NotImplementedException();
        }

        private static void ValidarCategoria(string categoria, List<ValidationResult> errores)
        {
            throw new NotImplementedException();
        }

        private static void ValidarNombre(string nombre, List<ValidationResult> errores)
        {
            throw new NotImplementedException();
        }
    }
}
