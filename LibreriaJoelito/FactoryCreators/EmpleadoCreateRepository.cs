using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;

namespace LibreriaJoelito.FactoryCreators
{
    public class EmpleadoCreateRepository : CreatorRepository<Empleado>
    {
        public override IRepository<Empleado> CreateRepository()
        {
            return new EmpleadoRepository();
        }
    }
}
