using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Servicios
{
    public class ServicioUsuario:IServicioUsuario
    {
        private readonly IRepository<Usuario> _usuarioRepo;
        private readonly IUsuarioRepository _extraRepo;
        private readonly IPasswordHasher _passwordHasher;

        public ServicioUsuario(IRepository<Usuario> usuarioRepo, IUsuarioRepository extraRepo,IPasswordHasher passwordHasher)
        {
            _usuarioRepo = usuarioRepo;
            _extraRepo = extraRepo;
            _passwordHasher = passwordHasher;
        }
        public int InsertUsuario(Usuario usuario)
        {
            usuario.Password = _passwordHasher.Hash(usuario.Password);
            return _usuarioRepo.Insert(usuario);

            
        }
        public int UpdateUsuario(Usuario usuario)
        {
            return _usuarioRepo.Update(usuario);
        }
        public int DeleteUsuario(Usuario t)
        {
            return _usuarioRepo.Delete(t);

        }
        public DataTable GetAllUsuarios()
        {
            return _usuarioRepo.GetAll();
        }
        public DataRow GetUsuarioById(int id)
        {
            return new DataTable().NewRow();
        }

        public bool ExisteUsuarioDuplicado(Usuario usuario)
        {
            return _usuarioRepo.ExisteDuplicado(usuario);
        }
        public string GenerarUsername(string nombre, string apellido)
        {
            string baseUsername = $"{nombre}.{apellido}".ToLower().Replace(" ", "");
            string username = baseUsername;
            int counter = 1;

            while (_extraRepo.ExisteUsername(username))
            {
                username = baseUsername + counter;
                counter++;
            }

            return username;

        }
        public string GenerarPassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%";
            var random = new Random();

            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public LoginResult Login(string username, string password)
        {
            var loginResult = new LoginResult();
            string userPassword = _extraRepo.GetPasswordByUsername(username);

            if (userPassword == null)
            {
                loginResult.Success = false;
                loginResult.Message = "NO se encontro a ese usuario";
                loginResult.Token = null;
            }
            if(_passwordHasher.Verify(password, userPassword))
            {
                loginResult.Success = true;
                loginResult.Message = "Cuenta verificada correctamente se envio el jwt";
                //aqui se manda el jwt 
                loginResult.Token = "jwt";
            }
            else
            {
                loginResult.Success = false;
                loginResult.Message = "Contrase;a incorrecta";
                loginResult.Token = null;
            }

            return loginResult;
        }

        

    }
}
