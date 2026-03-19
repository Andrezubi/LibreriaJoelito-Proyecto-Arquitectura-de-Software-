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
            ValidarCategoria(producto.IdCategoria, errores);
            ValidarIdMarca(producto.IdMarca, errores);
            ValidarStock(producto.Stock, errores);
            

            return errores;
        }

        public static void ValidarNombre(string nombre, List<ValidationResult> errores)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                errores.Add(new ValidationResult("El nombre es obligatorio.", new[] { "Nombre" }));

            else if (nombre.Length > 150)
                errores.Add(new ValidationResult("El nombre no puede tener más de 150 caracteres.", new[] { "Nombre" }));
        }

        public static void ValidarCategoria(int idCategoria, List<ValidationResult> errores)
        {
            if (idCategoria==0)
                errores.Add(new ValidationResult("La categoría es obligatoria.", new[] { "Categoria" }));
        }

        
        public static void ValidarStock(int stock, List<ValidationResult> errores)
        {
            if (stock < 0)
                errores.Add(new ValidationResult("El stock no puede ser negativo.", new[] { "Stock" }));
        }

        public static void ValidarIdMarca(int idMarca, List<ValidationResult> errores)
        {
            if (idMarca == 0)
                errores.Add(new ValidationResult("La categoría es obligatoria.", new[] { "Categoria" }));

        }
    }
}
