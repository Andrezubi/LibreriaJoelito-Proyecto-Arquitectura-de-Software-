using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;

namespace LibreriaJoelito.Infraestructura.FactoryCreators
{
    public class MarcaCreatorRepository: CreatorRepository<Marca>
    {
        public override IRepository<Marca> CreateRepository()
        {
            return new MarcaRepository();
        }
    }
}
