using LibreriaJoelito.Models;
using MySql.Data.MySqlClient;
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
            ValidarIdProductoBase(producto.IdProductoBase, errores,producto.Id);

            return errores;
        }

        public static void ValidarNombre(string nombre, List<ValidationResult> errores)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                errores.Add(new ValidationResult("El nombre es obligatorio.", new[] { "Nombre" }));

            else if (nombre.Length > 150)
                errores.Add(new ValidationResult("El nombre no puede tener más de 150 caracteres.", new[] { "Nombre" }));
        }

        public static void ValidarCategoria(string categoria, List<ValidationResult> errores)
        {
            if (string.IsNullOrWhiteSpace(categoria))
                errores.Add(new ValidationResult("La categoría es obligatoria.", new[] { "Categoria" }));

            else if (categoria.Length > 100)
                errores.Add(new ValidationResult("La categoría no puede tener más de 100 caracteres.", new[] { "Categoria" }));
        }

        public static void ValidarPrecio(decimal precio, List<ValidationResult> errores)
        {
            if (precio <= 0)
                errores.Add(new ValidationResult("El precio debe ser mayor a 0.", new[] { "Precio" }));
        }

        public static void ValidarStock(int stock, List<ValidationResult> errores)
        {
            if (stock < 0)
                errores.Add(new ValidationResult("El stock no puede ser negativo.", new[] { "Stock" }));
        }

        public static void ValidarTipoVenta(string tipoVenta, List<ValidationResult> errores)
        {
            if (string.IsNullOrWhiteSpace(tipoVenta))
                errores.Add(new ValidationResult("El tipo de venta es obligatorio.", new[] { "TipoVenta" }));

            else if (tipoVenta.Length > 50)
                errores.Add(new ValidationResult("El tipo de venta no puede tener más de 50 caracteres.", new[] { "TipoVenta" }));
        }

        public static void ValidarFactorConversion(decimal? factorConversion, List<ValidationResult> errores)
        {
            if (factorConversion.HasValue && factorConversion <= 0)
                errores.Add(new ValidationResult("El factor de conversión debe ser mayor a 0.", new[] { "FactorConversion" }));
        }

        public static void ValidarIdProductoBase(int? idProductoBase, List<ValidationResult> errores,int id)
        {
            if (idProductoBase.HasValue && idProductoBase <= 0)
                errores.Add(new ValidationResult("El Id del producto base no es válido.", new[] { "IdProductoBase" }));

            if (idProductoBase.HasValue && !encontroProducto(idProductoBase.Value))
                errores.Add(new ValidationResult("El producto base no existe.", new[] { "IdProductoBase" }));
            if (idProductoBase.HasValue && idProductoBase == id)
                errores.Add(new ValidationResult("El producto no puede ser su propio producto base", new[] { "IdProductoBase" }));
        }

        public static bool encontroProducto(int id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT 1 FROM productos WHERE Id=@id LIMIT 1");
            cmd.Parameters.AddWithValue("@id", id);

            using (MySqlDataReader reader = RepositorioBD.ExecuteReader(cmd))
            {
                return reader.Read();
            }
        }
    }
}
