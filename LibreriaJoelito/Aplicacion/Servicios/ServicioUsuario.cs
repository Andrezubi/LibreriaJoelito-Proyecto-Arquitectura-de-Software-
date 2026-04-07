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
        private readonly ITokenService _tokenService;

        public ServicioUsuario(IRepository<Usuario> usuarioRepo, IUsuarioRepository extraRepo,IPasswordHasher passwordHasher, ITokenService tokenService)
        {
            _usuarioRepo = usuarioRepo;
            _extraRepo = extraRepo;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
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
            var datosUsuario = _extraRepo.GetDatosLogin(username); 

            if (datosUsuario == null)
            {
                return new LoginResult { Success = false, Message = "Usuario no encontrado" };
            }

            // 2. Verificar password usando el hasher actual (SimpleHasher o BCrypt)
            if (_passwordHasher.Verify(password, datosUsuario.Password))
            {
                // 3. Generar el JWT real usando el Rol de la BD
                string token = _tokenService.GenerarToken(username, datosUsuario.Rol,datosUsuario.IdUsuario.ToString());

                return new LoginResult {
                    Success = true,
                    Message = "Acceso concedido",
                    Token = token
                };
            }

            return new LoginResult { Success = false, Message = "Contraseña incorrecta" };
        }

        

    }
}
