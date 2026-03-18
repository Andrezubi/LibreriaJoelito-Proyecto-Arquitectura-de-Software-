namespace LibreriaJoelito.Models
{
    public class Cliente: Usuario
    {
        
        public bool ClienteFrecuente { get; set; }
        public bool Estado { get; set; } = true;
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaUltimaActualizacion { get; set; }
        public int? IdEmpleadoCambio { get; set; }

        public Cliente() { }

        public Cliente(string nombre, string apellidoPaterno, string? apellidoMaterno, string cI, string? complemento, string? email, bool clienteFrecuente)
            : base(nombre, apellidoPaterno, apellidoMaterno, cI, complemento, email)
        {
            ClienteFrecuente = clienteFrecuente;
        }
    }
}
