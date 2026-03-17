namespace LibreriaJoelito.Models
{
    public class Empleado: Usuario
    {
        public Empleado() { }

        public Empleado(string nombre, string apellidoPaterno, string? apellidoMaterno, string ci, string email)
            : base(nombre, apellidoPaterno, apellidoMaterno, ci, email)
        {
        }

    }
}
