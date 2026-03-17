namespace LibreriaJoelito.Models
{
    public class Usuario
    {
        

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CI { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string? Email { get; set; }

        public Usuario() { }

        public Usuario(string nombre, string apellido, string cI, string? email)
        {
            Nombre = nombre;
            Apellido = apellido;
            CI = cI;
            Email = email;
        }
    }
}
