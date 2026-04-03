using LibreriaJoelito.Aplicacion.Interfaces;

namespace LibreriaJoelito.Infraestructura.FactoryCreators
{
    public abstract class CreatorRepository<T>
    {
        public abstract IRepository<T> CreateRepository();
    }
}
