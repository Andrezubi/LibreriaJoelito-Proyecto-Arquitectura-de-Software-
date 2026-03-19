namespace LibreriaJoelito.Models
{
    public class Producto
    {
        public int Id { get; set; }

        public int IdCategoria { get; set; }
        public int IdMarca { get; set; }
        public string Nombre { get; set; }

        public int Stock { get; set; }
        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }

        public DateTime? FechaUltimaActualizacion { get; set; }

        public int? IdEmpleadoCambio { get; set; }

        public Producto()
        {

        }
        public Producto(int id)// constructor para Delete
        {
            Id = id;
        }

        public Producto(int idCategoria,int idMarca, string nombre, int stock)//constructor para insert
        {
            IdCategoria = idCategoria;
            IdMarca = idMarca;
            Nombre = nombre;
            Stock = stock;
            Estado = true;
            FechaRegistro = DateTime.Now;
            FechaUltimaActualizacion = DateTime.Now;
        }
        public Producto(int id,int idCategoria, int idMarca, string nombre, int stock)//constructor para update
        {
            Id = id;
            IdCategoria = idCategoria;
            IdMarca = idMarca;
            Nombre = nombre;
            Stock = stock;
            Estado = true;
            FechaRegistro = DateTime.Now;
            FechaUltimaActualizacion = DateTime.Now;
        }
    }
}
