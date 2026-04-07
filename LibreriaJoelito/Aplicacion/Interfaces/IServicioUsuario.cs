using LibreriaJoelito.Dominio.Models;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Interfaces
{
    public interface IServicioUsuario
    {
        int InsertUsuario(Usuario usuario);

        int UpdateUsuario(Usuario usuario);

        int DeleteUsuario(Usuario t);

        DataTable GetAllUsuarios();
        DataRow GetUsuarioById(int id);

        bool ExisteUsuarioDuplicado(Usuario usuario);
        string GenerarUsername(string nombre, string apellido);
        string GenerarPassword(int length);

        LoginResult Login(string username,string password);
       
    }
}
