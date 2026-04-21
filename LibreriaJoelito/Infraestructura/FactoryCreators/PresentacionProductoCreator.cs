using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;

namespace LibreriaJoelito.Infraestructura.FactoryCreators
{
    public class PresentacionProductoCreator:CreatorRepository<PresentacionProducto>
    {
        public override IRepository<PresentacionProducto> CreateRepository()
        {
            return new PresentacionProductoRepository();
        }
    }
}
