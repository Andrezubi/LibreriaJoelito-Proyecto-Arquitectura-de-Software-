namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IPdfService
    {
        byte[] GenerarComprobanteVenta(System.Data.DataTable datosVenta);
    }
}