using LibreriaJoelito.Dominio.Models;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IProductoRepository : IRepository<Producto>
    {
        int DescontarStock(int idProducto, int cantidad);

        DataTable BuscarProducto(string nombre);

        DataTable BuscarPorNombre(string frase);
    }
}
