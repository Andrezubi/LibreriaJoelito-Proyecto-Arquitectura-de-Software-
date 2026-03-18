namespace LibreriaJoelito.Models
{
    public class Empleado: Usuario
    {
        public Empleado() { }
        public Empleado(int id) { }

        public Empleado(string nombre, string apellidoPaterno, string? apellidoMaterno, string ci, string? complemento, string? email)
            : base(nombre, apellidoPaterno, apellidoMaterno, ci, complemento, email)
        {
        }

    }
}
