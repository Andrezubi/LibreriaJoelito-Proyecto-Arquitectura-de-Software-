using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;

namespace LibreriaJoelito.FactoryCreators
{
    public class ProductoCreatorRepository:CreatorRepository<Producto>
    {
        public override IRepository<Producto> CreateRepository()
        {
            return new ProductoRepository();
        }
    }
}
