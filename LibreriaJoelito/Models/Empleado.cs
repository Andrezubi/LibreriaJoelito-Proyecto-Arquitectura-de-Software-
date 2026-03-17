namespace LibreriaJoelito.Models
{
    public class Empleado: Usuario
    {
        public Empleado() { }

        public Empleado(string nombre, string apellido, string ci, string email)
            : base(nombre, apellido, ci, email)
        {
        }

    }
}
