using LibreriaJoelito.Models;
using System.ComponentModel.DataAnnotations;

namespace LibreriaJoelito.Validators
{
    public static class ClienteValidator
    {
        public static List<ValidationResult> Validar(Cliente cliente)
        {
            var errores = new List<ValidationResult>();

            ValidarNombre(cliente.Nombre, errores);
            //ValidarApellido(cliente.Apellido, errores);
            ValidarCI(cliente.CI, errores);
            ValidarComplemento(cliente.Complemento, errores);
            ValidarEmail(cliente.Email, errores);

            return errores;
        }

        static void ValidarNombre(string nombre, List<ValidationResult> errores)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                errores.Add(new ValidationResult("El Nombre es obligatorio.", new[] { "Cliente.Nombre" }));
        }

        static void ValidarApellido(string apellido, List<ValidationResult> errores)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                errores.Add(new ValidationResult("El Apellido es obligatorio.", new[] { "Cliente.Apellido" }));
        }

        static void ValidarCI(string ci, List<ValidationResult> errores)
        {
            if (string.IsNullOrWhiteSpace(ci))
                errores.Add(new ValidationResult("El CI es obligatorio.", new[] { "Cliente.CI" }));
            else if (ci.Length < 6)
                errores.Add(new ValidationResult("El CI debe tener al menos 6 caracteres.", new[] { "Cliente.CI" }));
        }

        static void ValidarEmail(string? email, List<ValidationResult> errores)
        {
            if (!string.IsNullOrWhiteSpace(email) && !email.Contains("@"))
                errores.Add(new ValidationResult("El Email no tiene un formato válido.", new[] { "Cliente.Email" }));
        }

        static void ValidarComplemento(string? complemento, List<ValidationResult> errores)
        {
            if (!string.IsNullOrWhiteSpace(complemento) && complemento.Length > 10)
                errores.Add(new ValidationResult("El Complemento no puede exceder los 10 caracteres.", new[] { "Cliente.Complemento" }));
        }
    }
}
