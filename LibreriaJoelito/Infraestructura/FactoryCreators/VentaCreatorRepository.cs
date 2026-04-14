using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;

namespace LibreriaJoelito.Infraestructura.FactoryCreators
{
    public class VentaCreatorRepository : CreatorRepository<Venta>
    {
        public override IRepository<Venta> CreateRepository()
        {
            return new VentaRepository();
        }
    }
}
