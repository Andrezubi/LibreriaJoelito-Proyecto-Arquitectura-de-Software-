using LibreriaJoelito.Dominio.Models;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IUsuarioRepository:IRepository<Usuario>
    {
        bool ExisteUsername(string username);
        string GetPasswordByUsername(string username);
    }
}
