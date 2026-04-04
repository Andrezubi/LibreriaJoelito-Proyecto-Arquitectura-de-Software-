using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;

namespace LibreriaJoelito.Infraestructura.FactoryCreators
{
    public class ProductoCreatorRepository:CreatorRepository<Producto>
    {
        public override IRepository<Producto> CreateRepository()
        {
            return new ProductoRepository();
        }
    }
}
