using System.ComponentModel.DataAnnotations;
using LibreriaJoelito.Models;
using System.Net.Mail;

namespace LibreriaJoelito.Validators
{
    public static class EmpleadoValidator
    {
        public static List<ValidationResult> Validar(Empleado emp)
        {
            var errores = new List<ValidationResult>();

            // 1. Nombre
            if (!esNombreValido(emp.Nombre))
                errores.Add(new ValidationResult("El nombre no es válido (mínimo 2 caracteres y sin espacios a los lados).", new[] { "Nombre" }));

            // 2. Apellido
            if (!esApellidoValido(emp.ApellidoPaterno))
                errores.Add(new ValidationResult("El apellido no es válido (mínimo 4 caracteres).", new[] { "ApellidoPaterno" }));

            // 3. CI
            if (!esCiValido(emp.Ci))
                errores.Add(new ValidationResult("El CI debe tener entre 6 y 11 dígitos.", new[] { "Ci" }));

            // 4. Extensión Carnet
            if (!esExtensionCarnetValida(emp.Complemento))
                errores.Add(new ValidationResult("La extensión del carnet debe estar compuesta de un número y una letra.", new[] { "ExtensionCi" }));

            // 5. Correo
            if (!esCorreoValido(emp.Email))
                errores.Add(new ValidationResult("El formato del correo electrónico no es correcto.", new[] { "Email" }));

            // 6. Fecha Nacimiento
            if (!esFechaNacimientoValida(emp.FechaNacimiento))
                errores.Add(new ValidationResult("La fecha de nacimiento no es válida (debe ser mayor de 18 años).", new[] { "FechaNacimiento" }));

            // 7. Teléfono
            if (!esTelefonoValido(emp.Telefono))
                errores.Add(new ValidationResult("El número de teléfono debe tener 8 dígitos y no empezar con 0, 5 o 9.", new[] { "Telefono" }));

            // 8. Dirección
            if (!esDireccionValida(emp.DireccionDomicilio))
                errores.Add(new ValidationResult("La dirección no es válida (mínimo 10 caracteres).", new[] { "DireccionDomicilio" }));

            // 9. Fecha Ingreso
            if (!esFechaIngresoValida(emp.FechaIngreso))
                errores.Add(new ValidationResult("La fecha de ingreso no es válida (no puede ser una fecha futura).", new[] { "FechaIngreso" }));

            return errores;
        }

        #region funciones de Validacion

        private static bool esNombreValido(string nombre) =>
            !string.IsNullOrWhiteSpace(nombre) && nombre == nombre.Trim() && nombre.Length >= 2;

        private static bool esApellidoValido(string apellido) =>
            !string.IsNullOrWhiteSpace(apellido) && apellido.Length >= 4;

        private static bool esCiValido(string ci) =>
            !string.IsNullOrEmpty(ci) && ci.All(char.IsDigit) && ci.Length >= 6 && ci.Length <= 11;

        private static bool esExtensionCarnetValida(string ext) =>
            string.IsNullOrEmpty(ext) || (ext.Length == 2 && char.IsDigit(ext[0]) && char.IsLetter(ext[1]));

        private static bool esFechaNacimientoValida(DateOnly fecha)
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            int edad = hoy.Year - fecha.Year;
            if (fecha > hoy.AddYears(-edad)) edad--;
            return edad >= 18 && edad <= 65;
        }

        private static bool esCorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo)) return true;
            try { new MailAddress(correo); return true; } catch { return false; }
        }

        private static bool esDireccionValida(string dir) =>
            string.IsNullOrWhiteSpace(dir) || (dir.Length >= 10 && dir.Length <= 120);

        private static bool esTelefonoValido(string tel)
        {
            if (string.IsNullOrWhiteSpace(tel)) return true;
            return tel.All(char.IsDigit) && tel.Length == 8 && !new[] { '0', '5', '9' }.Contains(tel[0]);
        }

        private static bool esFechaIngresoValida(DateOnly? fecha) =>
            !fecha.HasValue || fecha.Value <= DateOnly.FromDateTime(DateTime.Today);

        #endregion
    }
}