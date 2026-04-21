using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;

namespace LibreriaJoelito.Infraestructura.FactoryCreators
{
    public class PresentacionCreatorRepository : CreatorRepository<LibreriaJoelito.Dominio.Models.Presentacion>
    {
        public override IRepository<LibreriaJoelito.Dominio.Models.Presentacion> CreateRepository()
        {
            return new PresentacionRepository();
        }
    }
}