using LibreriaJoelito.Dominio.Models;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IPresentacionProductoRepository:IRepository<PresentacionProducto>
    {
        public DataTable obtenerPresentacionProductoDetallado(string frase);

        public DataRow? GetByIds(int idProducto, int idPresentacion);

        int InsertarRelacion(int idProducto, int idPresentacion, double factorConversion, decimal precio, int? idUsuario);


    }
}
