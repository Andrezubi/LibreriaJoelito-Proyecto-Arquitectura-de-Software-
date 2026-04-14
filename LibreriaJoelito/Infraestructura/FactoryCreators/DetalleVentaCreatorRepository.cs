using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;

namespace LibreriaJoelito.Infraestructura.FactoryCreators
{
    public class DetalleVentaCreatorRepository : CreatorRepository<DetalleVenta>
    {
        public override IRepository<DetalleVenta> CreateRepository()
        {
            return new DetalleVentaRepository();
        }
    }
}
