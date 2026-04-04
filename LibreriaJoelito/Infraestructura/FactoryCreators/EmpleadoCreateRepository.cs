using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;

namespace LibreriaJoelito.Infraestructura.FactoryCreators
{
    public class EmpleadoCreateRepository : CreatorRepository<Empleado>
    {
        public override IRepository<Empleado> CreateRepository()
        {
            return new EmpleadoRepository();
        }
    }
}
