using LibreriaJoelito.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

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

            if (nombre.Length < 2)
                errores.Add(new ValidationResult("El nombre debe tener al menos 2 caracteres.", new[] { "Nombre" }));

            if (nombre.Length > 150)
                errores.Add(new ValidationResult("El nombre no puede tener más de 150 caracteres.", new[] { "Nombre" }));
            if (!Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                errores.Add(new ValidationResult(
                    "Nombre no puede contener Caracteres Invalidos.",
                    new[] { "Nombre" }));
            }
        }

        public static void ValidarCategoria(int idCategoria, List<ValidationResult> errores)
        {
            if (idCategoria <= 0)
                errores.Add(new ValidationResult("La categoría es obligatoria.", new[] { "IdCategoria" }));
        }

        public static void ValidarStock(int stock, List<ValidationResult> errores)
        {
            if (stock < 0)
                errores.Add(new ValidationResult("El stock no puede ser negativo.", new[] { "Stock" }));
            if (stock > 100000000)
                errores.Add(new ValidationResult("El stock no puede sobrepasar los 100000000"));
        }

        public static void ValidarIdMarca(int idMarca, List<ValidationResult> errores)
        {
            if (idMarca <= 0)
                errores.Add(new ValidationResult("La marca es obligatoria.", new[] { "IdMarca" }));
        }

        public static bool ContieneCaracteresInvalidos(string texto)
        {
            return !Regex.IsMatch(texto, @"^[a-zA-Z0-9áéíóúÁÉÍÓÚñÑ\s\-\.\&\(\),\/]+$");
        }
    }
}
