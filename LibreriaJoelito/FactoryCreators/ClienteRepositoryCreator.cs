using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;

namespace LibreriaJoelito.FactoryCreators
{
    public class ClienteRepositoryCreator : CreatorRepository<Cliente>
    {
        public override IRepository<Cliente> CreateRepository()
        {
            return new ClienteRepository();
        }
    }
}
