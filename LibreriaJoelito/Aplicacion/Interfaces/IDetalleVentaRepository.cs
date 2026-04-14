using LibreriaJoelito.Dominio.Models;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IDetalleVentaRepository : IRepository<DetalleVenta>
    {
        DataTable GetByIdVenta(int ventaId);
        int DeleteByIdVenta(int ventaId);
    }
}
