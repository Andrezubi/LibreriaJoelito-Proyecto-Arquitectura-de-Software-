namespace LibreriaJoelito.Models
{
    public class Usuario
    {
        

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string? ApellidoMaterno { get; set; }
        public string Ci { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string? Email { get; set; }

        public Usuario() { }

        public Usuario(string nombre, string apellidoPaterno, string? apellidoMaterno, string cI, string? complemento, string? email)
        {
            Nombre = nombre;
            ApellidoPaterno = apellidoPaterno;
            ApellidoMaterno = apellidoMaterno;
            Ci = cI;
            Complemento = complemento;
            Email = email;
        }
    }
}
