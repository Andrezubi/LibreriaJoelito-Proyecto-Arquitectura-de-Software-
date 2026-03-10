namespace LibreriaJoelito.Validators
{
    public class EmpleadoValidator
    {
        public static bool esCorreoValido(string correo)
        {
            return correo.Contains("@");
        }

        public static bool esCiValido(string ci)
        {
            return ci.Length > 6;
        }

        public static bool esNombreValido(string nombre)
        {
            if (nombre != nombre.Trim())
            {
                return false;
            }

            return nombre.Length > 3;
        }
    }
}
