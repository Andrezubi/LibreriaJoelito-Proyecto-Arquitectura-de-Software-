namespace LibreriaJoelito.Models
{
    public class Cliente: Usuario
    {
        
        public bool EsClienteFrecuente { get; set; }
        public bool Estado { get; set; } = true;
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaUltimaActualizacion { get; set; }
        public int? IdEmpleadoCambio { get; set; }

        public Cliente() { }

        public Cliente(string nombre, string apellido, string ci, string email, bool esClienteFrecuente)
            :base(nombre, apellido, ci, email)
        {
            EsClienteFrecuente = esClienteFrecuente;
        }
    }
}
