using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;

namespace LibreriaJoelito.Infraestructura.FactoryCreators
{
    public class ClienteRepositoryCreator : CreatorRepository<Cliente>
    {
        public override IRepository<Cliente> CreateRepository()
        {
            return new ClienteRepository();
        }
    }
}
