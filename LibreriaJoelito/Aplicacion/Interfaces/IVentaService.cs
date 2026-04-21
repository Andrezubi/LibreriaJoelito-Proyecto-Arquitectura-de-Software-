using LibreriaJoelito.Aplicacion.Results;
using LibreriaJoelito.Dominio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IVentaService
    {
        Result<int> RegistrarVenta(Venta venta, List<DetalleVenta> detalles);
        Result AnularVenta(int idVenta);


        DataTable getPresentacionProductosByFrase(string frase);
        JsonResult getPresentacionProductoByIds(int idProducto,int  idPresentacion);
    }
}
