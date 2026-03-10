namespace LibreriaJoelito.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CI { get; set; } = string.Empty;
        public string Complemento { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EsClienteFrecuente { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimaActualizacion { get; set; }

        public Cliente() { }

        public Cliente(string nombre, string apellido, string ci, string email, bool esClienteFrecuente)
        {
            Nombre = nombre;
            Apellido = apellido;
            CI = ci;
            Email = email;
            EsClienteFrecuente = esClienteFrecuente;
        }
    }
}
