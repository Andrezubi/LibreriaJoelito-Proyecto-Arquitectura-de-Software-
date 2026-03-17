namespace LibreriaJoelito.Models
{
    public class Cliente: Usuario
    {
        
        public bool EsClienteFrecuente { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimaActualizacion { get; set; }

        public Cliente() { }

        public Cliente(string nombre, string apellido, string ci, string email, bool esClienteFrecuente)
            :base(nombre, apellido, ci, email)
        {
            EsClienteFrecuente = esClienteFrecuente;
        }
    }
}
