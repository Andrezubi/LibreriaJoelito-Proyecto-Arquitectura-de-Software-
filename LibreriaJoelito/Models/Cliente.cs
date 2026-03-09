namespace LibreriaJoelito.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string CI { get; set; }
        public string Email { get; set; }
        public bool EsClienteFrecuente { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimaActualizacion { get; set; }

        public Cliente() { }

        public Cliente(string nombre, string apellido, string ci, string email)
        {
            Nombre = nombre;
            Apellido = apellido;
            CI = ci;
            Email = email;
        }
    }
}
