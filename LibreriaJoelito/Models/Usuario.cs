namespace LibreriaJoelito.Models
{
    public class Usuario
    {
        

        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string ApellidoPaterno { get; set; } = string.Empty;
        public string? ApellidoMaterno { get; set; }
        public string CI { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public string? Email { get; set; }

        public Usuario() { }

        public Usuario(string nombre, string apellidoPaterno, string? apellidoMaterno, string cI, string? complemento, string? email)
        {
            Nombre = nombre;
            ApellidoPaterno = apellidoPaterno;
            ApellidoMaterno = apellidoMaterno;
            CI = cI;
            Complemento = complemento;
            Email = email;
        }
    }
}
