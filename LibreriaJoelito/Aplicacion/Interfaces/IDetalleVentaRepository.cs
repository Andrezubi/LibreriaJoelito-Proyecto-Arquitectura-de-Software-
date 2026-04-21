using LibreriaJoelito.Dominio.Models;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IDetalleVentaRepository : IRepository<DetalleVenta>
    {
        DataTable GetDetalleExtraByIdVenta(int idVenta);
        DataTable GetByIdVenta(int ventaId);
        int DeleteByIdVenta(int ventaId);
    }
}
