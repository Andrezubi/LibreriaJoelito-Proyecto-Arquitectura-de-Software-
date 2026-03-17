namespace LibreriaJoelito.Validators
{
    public class EmpleadoValidator
    {
        #region métodos
        public static bool esNombreValido(string nombre)
        {
            if (nombre != nombre.Trim())
            {
                return false;
            }

            return nombre.Length > 1 && nombre.Length < 748;
        }
        public static bool esApellidoValido(string apellido)
        {
            if (string.IsNullOrEmpty(apellido))
                return false;

            return apellido.Length > 3 && apellido.Length < 36;
        }

        

        public static bool esCiValido(string ci)
        {
            if (!ci.All(char.IsDigit))
            {
                return false;
            }
            return ci.Length == 8;
        }    
        
        public static bool esExtensionCarnetValida(string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return true;

            return extension.Length == 2 && char.IsDigit(extension[0]) && char.IsLetter(extension[1]);

        }
        public static int calcularEdad(DateOnly fechaNacimiento)
        {
            DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);
            int edad = hoy.Year - fechaNacimiento.Year;
            if (hoy < fechaNacimiento.AddYears(edad))
            {
                edad--;
            }
            return edad;
        }
        public static bool esFechaNacimientoValida(DateOnly fechaNacimiento)
        {
            int edad = calcularEdad(fechaNacimiento);
            return edad >= 18 && edad <= 65;
        }

        public static int calcularEdadUpdate(DateTime fechaNacimiento)
        {
            DateTime hoy = DateTime.Now;
            int edad = hoy.Year - fechaNacimiento.Year;
            if (hoy < fechaNacimiento.AddYears(edad))
            {
                edad--;
            }
            return edad;
        }
        public static bool esFechaNacimientoValidaUpdate(DateTime fechaNacimiento)
        {
            int edad = calcularEdadUpdate(fechaNacimiento);
            return edad >= 18 && edad <= 65;
        }

        public static bool esCorreoValido(string correo)
        {
            return correo.Contains("@");
        }
        public static bool esDireccionValida(string direccion)
        {
            if (string.IsNullOrEmpty(direccion))
                return true;

            return direccion.Length > 9 && direccion.Length < 121;
        }
        
        public static bool esTelefonoValido(int telefono)
        {
            if(telefono == 0)
                return true;

            if (telefono.ToString()[0] == '9' || telefono.ToString()[0] == '5' || telefono.ToString()[0] == '0')
            {
                return false;
            }
            return telefono > 1000000 && telefono < 99999999;
        }
        public static bool esFechaIngresoValida(DateOnly? fechaIngreso)
        {
            if (!fechaIngreso.HasValue)
                return true;

            DateOnly hoy = DateOnly.FromDateTime(DateTime.Now);
            return fechaIngreso <= hoy;
        }

        public static bool esFechaIngresoValidaUpdate(DateTime? fechaIngreso)
        {
            if (!fechaIngreso.HasValue)
                return true;

            DateTime hoy = DateTime.Now;
            return fechaIngreso <= hoy;
        }
        #endregion
    }
}
