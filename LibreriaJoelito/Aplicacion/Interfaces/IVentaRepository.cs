using LibreriaJoelito.Aplicacion.Results;
using LibreriaJoelito.Dominio.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IVentaRepository : IRepository<Venta>
    {
        DataTable GetByDate(DateTime fechaInicio, DateTime fechaFin);
        DataTable GetByIdCliente(int idCliente);
    }
}
