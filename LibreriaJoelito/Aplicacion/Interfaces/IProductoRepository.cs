using LibreriaJoelito.Dominio.Models;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IProductoRepository : IRepository<Producto>
    {
        int DescontarStock(int idProducto, int cantidad);
    }
}
