using LibreriaJoelito.Aplicacion.Interfaces;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Servicios
{
    public class PresentacionServicio
    {

        private readonly IPresentacionRepository _presentacionRepo;


        public PresentacionServicio(IPresentacionRepository presentacionRepo)
        {
            _presentacionRepo = presentacionRepo;
        }

        public DataTable GetAll()
        {
            return _presentacionRepo.GetAll();
        }
    }
}
