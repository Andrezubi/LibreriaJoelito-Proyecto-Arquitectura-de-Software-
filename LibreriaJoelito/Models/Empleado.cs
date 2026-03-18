namespace LibreriaJoelito.Models
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string ApellidoPaterno { get; set; }
        public string ApellidoMaterno { get; set; }
        public string Ci { get; set; }
        public string Complemento { get; set; }
        public string DireccionDomicilio { get; set; }
        public string Email { get; set; }
        public int Telefono { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public DateOnly FechaIngreso { get; set; }

<<<<<<< HEAD
        public Empleado(string nombre, string apellidoPaterno, string? apellidoMaterno, string ci, string? complemento, string? email)
            : base(nombre, apellidoPaterno, apellidoMaterno, ci, complemento, email)
=======
        public Empleado() { }
        public Empleado(int id) { Id = id; }

        public Empleado(int id, string nombre, string apellidoPaterno, string apellidoMaterno, string ci, string complemento, string direccionDomicilio, string email, int telefono, DateOnly fechaNacimiento, DateOnly fechaIngreso)
>>>>>>> 019a7adaae8de863a50bc79165c3f778499acbf6
        {
            Id = id;
            Nombre = nombre;
            ApellidoPaterno = apellidoPaterno;
            ApellidoMaterno = apellidoMaterno;
            Ci = ci;
            Complemento = complemento;
            DireccionDomicilio = direccionDomicilio;
            Email = email;
            Telefono = telefono;
            FechaNacimiento = fechaNacimiento;
            FechaIngreso = fechaIngreso;
        }

        public Empleado(string nombre, string apellidoPaterno, string apellidoMaterno, string ci, string complemento, string direccionDomicilio, string email, int telefono, DateOnly fechaNacimiento, DateOnly fechaIngreso)
        {
            Nombre = nombre;
            ApellidoPaterno = apellidoPaterno;
            ApellidoMaterno = apellidoMaterno;
            Ci = ci;
            Complemento = complemento;
            DireccionDomicilio = direccionDomicilio;
            Email = email;
            Telefono = telefono;
            FechaNacimiento = fechaNacimiento;
            FechaIngreso = fechaIngreso;
        }
    }
}
