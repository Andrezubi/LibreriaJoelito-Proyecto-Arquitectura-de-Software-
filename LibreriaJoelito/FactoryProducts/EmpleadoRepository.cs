using LibreriaJoelito.Models;
using System.Data;

namespace LibreriaJoelito.FactoryProducts
{
    public class EmpleadoRepository : IRepository<Empleado>
    {
        public int Insert(Empleado t)
        {
            return 0;
        }
        public int Update(Empleado t)
        {
            return 0;
        }
        public int Delete(Empleado t)
        {
            return 0;
        }
        public DataTable GetAll()
        {
            return new DataTable();
        }
        public DataRow GetById(int id)
        {
            return new DataTable().NewRow();
        }
    }
}
