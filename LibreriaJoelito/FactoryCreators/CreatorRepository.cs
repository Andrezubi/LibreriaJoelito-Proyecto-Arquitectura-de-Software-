using LibreriaJoelito.FactoryProducts;

namespace LibreriaJoelito.FactoryCreators
{
    public abstract class CreatorRepository<T>
    {
        public abstract IRepository<T> CreateRepository();
    }
}
