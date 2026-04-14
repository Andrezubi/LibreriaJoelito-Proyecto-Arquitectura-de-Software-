using LibreriaJoelito.Aplicacion.Results;
using LibreriaJoelito.Dominio.Models;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IVentaService
    {
        Result<int> RegistrarVenta(Venta venta, List<DetalleVenta> detalles);
        Result AnularVenta(int idVenta);
    }
}
