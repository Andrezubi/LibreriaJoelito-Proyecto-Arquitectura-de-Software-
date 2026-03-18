using LibreriaJoelito.Models;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;

namespace LibreriaJoelito.Validators
{
    public static class ClienteValidator
    {
        public static string NormalizarTexto(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return string.Empty;
            texto = Regex.Replace(texto.Trim(), @"\s+", " ");
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(texto.ToLower());
        }

        public static List<ValidationResult> Validar(Cliente cliente)
        {
            var errores = new List<ValidationResult>();

            ValidarNombre(cliente.Nombre, errores);
            ValidarApellidoPaterno(cliente.ApellidoPaterno, errores);
            ValidarApellidoMaterno(cliente.ApellidoMaterno, errores);
            ValidarCI(cliente.Ci, errores);
            ValidarComplemento(cliente.Complemento, errores);
            ValidarEmail(cliente.Email, errores);

            return errores;
        }

        static void ValidarNombre(string nombre, List<ValidationResult> errores)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                errores.Add(new ValidationResult("El Nombre es obligatorio.", new[] { "Cliente.Nombre" }));
                return;
            }
            if (!Regex.IsMatch(nombre, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                errores.Add(new ValidationResult("El Nombre solo puede contener letras y espacios.", new[] { "Cliente.Nombre" }));
            }
        }

        static void ValidarApellidoPaterno(string apellido, List<ValidationResult> errores)
        {
            if (string.IsNullOrWhiteSpace(apellido))
            {
                errores.Add(new ValidationResult("El Apellido Paterno es obligatorio.", new[] { "Cliente.ApellidoPaterno" }));
                return;
            }
            if (!Regex.IsMatch(apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
            {
                errores.Add(new ValidationResult("El Apellido Paterno solo puede contener letras y espacios.", new[] { "Cliente.ApellidoPaterno" }));
            }
        }

        static void ValidarApellidoMaterno(string? apellido, List<ValidationResult> errores)
        {
            if (!string.IsNullOrWhiteSpace(apellido))
            {
                if (!Regex.IsMatch(apellido, @"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$"))
                {
                    errores.Add(new ValidationResult("El Apellido Materno solo puede contener letras y espacios.", new[] { "Cliente.ApellidoMaterno" }));
                }
            }
        }

        static void ValidarCI(string ci, List<ValidationResult> errores)
        {
            if (string.IsNullOrWhiteSpace(ci))
            {
                errores.Add(new ValidationResult("El CI es obligatorio.", new[] { "Cliente.CI" }));
            }
            else
            {
                if (ci.Length < 6)
                    errores.Add(new ValidationResult("El CI debe tener al menos 6 caracteres.", new[] { "Cliente.CI" }));
                if (!Regex.IsMatch(ci, @"^\d+$"))
                    errores.Add(new ValidationResult("El CI solo puede contener números.", new[] { "Cliente.CI" }));
            }
        }

        static void ValidarEmail(string? email, List<ValidationResult> errores)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    errores.Add(new ValidationResult("El Email no tiene un formato válido.", new[] { "Cliente.Email" }));
                }
            }
        }

        static void ValidarComplemento(string? complemento, List<ValidationResult> errores)
        {
            if (!string.IsNullOrWhiteSpace(complemento))
            {
                if (!Regex.IsMatch(complemento, @"^[0-9][A-Z]$"))
                {
                    errores.Add(new ValidationResult("El Complemento debe tener un formato de número seguido de letra mayúscula (ej: 1A).", new[] { "Cliente.Complemento" }));
                }
            }
        }
    }
}
