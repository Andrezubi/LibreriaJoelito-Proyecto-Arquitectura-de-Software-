namespace LibreriaJoelito.Models
{
    public class Producto
    {
        public int Id { get; set; }

        public string Categoria { get; set; }

        public string Nombre { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }

        public string TipoVenta { get; set; }

        public decimal? FactorConversion { get; set; }

        public int? IdProductoBase { get; set; }

        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime? UltimaActualizacion { get; set; }

        public int? IdUsuario { get; set; }

        public Producto()
        {

        }

        public Producto(string categoria, string nombre, decimal precio, int stock, string tipoVenta, decimal factorConversion,int? idProductoBase)
        {
            Categoria = categoria;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            TipoVenta = tipoVenta;
            FactorConversion = factorConversion;
            IdProductoBase = idProductoBase;
        }
    }
}
