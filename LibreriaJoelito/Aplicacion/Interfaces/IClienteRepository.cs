using LibreriaJoelito.Dominio.Models;
using System.Data;


namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IClienteRepository : IRepository<Cliente>
    {
        DataRow GetByCi(string ci);
        DataTable GetAllSimilarId(string ci);
    }
}
