using System;
using System.Linq;
using System.Net.Mail;

namespace LibreriaJoelito.Validators
{
    public class EmpleadoValidator
    {
        #region métodos
        public static bool esNombreValido(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                return false;

            if (nombre != nombre.Trim())
                return false;

            return nombre.Length >= 2 && nombre.Length <= 100;
        }

        public static bool esApellidoValido(string apellido)
        {
            if (string.IsNullOrWhiteSpace(apellido))
                return false;

            return apellido.Length >= 2 && apellido.Length <= 50;
        }

        public static bool esCiValido(string ci)
        {
            if (string.IsNullOrEmpty(ci))
                return false;

            return ci.All(char.IsDigit) && ci.Length == 8;
        }

        public static bool esExtensionCarnetValida(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return true;

            return extension.Length == 2 && char.IsDigit(extension[0]) && char.IsLetter(extension[1]);
        }

        public static int calcularEdad(DateOnly fechaNacimiento)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);
            int edad = hoy.Year - fechaNacimiento.Year;
            if (fechaNacimiento > hoy.AddYears(-edad)) edad--;
            return edad;
        }

        public static bool esFechaNacimientoValida(DateOnly fechaNacimiento)
        {
            int edad = calcularEdad(fechaNacimiento);
            return edad >= 18 && edad <= 65;
        }

        public static bool esCorreoValido(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return false;

            try
            {
                var mail = new MailAddress(correo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool esDireccionValida(string direccion)
        {
            if (string.IsNullOrWhiteSpace(direccion))
                return false;

            return direccion.Length >= 10 && direccion.Length <= 120;
        }

        public static bool esTelefonoValido(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
                return false;

            if (!telefono.All(char.IsDigit))
                return false;

            if (telefono.Length != 8)
                return false;

            char primerDigito = telefono[0];
            if (primerDigito == '0' || primerDigito == '5' || primerDigito == '9')
                return false;

            return true;
        }

        public static bool esFechaIngresoValida(DateOnly? fechaIngreso)
        {
            if (!fechaIngreso.HasValue)
                return true;

            DateOnly hoy = DateOnly.FromDateTime(DateTime.Today);
            return fechaIngreso.Value <= hoy;
        }
        #endregion
    }
}
