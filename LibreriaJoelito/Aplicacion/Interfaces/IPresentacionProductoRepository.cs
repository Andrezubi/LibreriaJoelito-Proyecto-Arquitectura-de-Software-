using LibreriaJoelito.Dominio.Models;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IPresentacionProductoRepository:IRepository<PresentacionProducto>
    {
        public DataTable obtenerPresentacionProductoDetallado(string frase);

        public DataRow? GetByIds(int idProducto, int idPresentacion);
    }
}
