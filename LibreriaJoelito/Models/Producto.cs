namespace LibreriaJoelito.Models
{
    public class Producto
    {
        public byte Id { get; set; }
        public string Categoria { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string TipoVenta { get; set; }
        public decimal FactorConversion { get; set; }
        public byte IdProductoBase { get; set; }
        public byte Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime UltimaActualizacion { get; set; }
        public byte IdUsuario { get; set; }

        public Producto()
        {

        }

        public Producto(string categoria, string nombre, decimal precio, int stock, string tipoVenta, decimal factorConversion)
        {
            Categoria = categoria;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            TipoVenta = tipoVenta;
            FactorConversion = factorConversion;
        }
    }
}
