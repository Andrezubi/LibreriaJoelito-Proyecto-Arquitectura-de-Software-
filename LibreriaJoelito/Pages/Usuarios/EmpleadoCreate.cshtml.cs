using Google.Protobuf.WellKnownTypes;
using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages.Usuarios
{
    public class EmpleadoCreateModel : PageModel
    {
        #region inyecciónDependencias
        private readonly IRepository<Usuario> _empleadoRepo;
        private readonly IServicioUsuario _servicioUsuario;

        public EmpleadoCreateModel(IRepository<Usuario> empleadoRepo, IServicioUsuario servicioUsuario )
        {
            _empleadoRepo = empleadoRepo;
            _servicioUsuario = servicioUsuario;
        }
        #endregion

        public string messageResult { get; set; } = string.Empty;

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string? ApellidoPaterno { get; set; } = string.Empty;

        [BindProperty]
        public string? ApellidoMaterno { get; set; } = string.Empty;

        [BindProperty]
        public string Ci { get; set; } = string.Empty;

        [BindProperty]
        public string? ExtensionCi { get; set; }

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string? DireccionDomicilio { get; set; } = string.Empty;

        [BindProperty]
        public string? Telefono { get; set; } 

        [BindProperty]
        public DateOnly FechaNacimiento { get; set; }

        [BindProperty]
        public DateOnly FechaIngreso{ get; set; }
        [BindProperty]
        public string Rol {  get; set; } 

        public void OnGet() { }

        public IActionResult OnPost()
        {
            //en vez de 1 en el constructor a;adir el id del usuario el cual haya creado al usuario
            Usuario empleado = new Usuario(Nombre, ApellidoPaterno, ApellidoMaterno, Ci, ExtensionCi, DireccionDomicilio, Email, Telefono, FechaNacimiento, FechaIngreso, _servicioUsuario.GenerarUsername(Nombre, ApellidoPaterno), _servicioUsuario.GenerarPassword(10),Rol,1);

            var resultados = EmpleadoValidator.Validar(empleado);

            if (resultados.Any())
            {
                TempData["CreateSucces"] = resultados.First().ErrorMessage;

                return Page();
            }

            if(_servicioUsuario.ExisteUsuarioDuplicado(empleado))
            {
                TempData["CreateSucces"] = "El empleado con ese CI ya existe";
                return Page();
            }


            if (_servicioUsuario.InsertUsuario(empleado) == 1)
            {
                TempData["SuccessMessage"] = "Empleado creado exitosamente.";
                return RedirectToPage("EmpleadoGet");
                //aqui hacer la logica de envio de correos con empleado.Username y empleado.Password
            }
            else
            {
                messageResult = "Error al registrar el empleado.";
                return Page();
            }
        }
    }
}
