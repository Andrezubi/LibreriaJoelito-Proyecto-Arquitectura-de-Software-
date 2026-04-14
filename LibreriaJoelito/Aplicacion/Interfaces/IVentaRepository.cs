using System.Data;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IVentaRepository //: IRepository<Venta>
    {
        DataTable GetByDate(DateTime fechaInicio, DateTime fechaFin);
        DataTable GetByIdCliente(int idCliente);
    }
}
