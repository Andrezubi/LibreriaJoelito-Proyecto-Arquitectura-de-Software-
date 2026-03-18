using Google.Protobuf.WellKnownTypes;
using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;
using LibreriaJoelito.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages.Empleados
{
    public class EmpleadoCreateModel : PageModel
    {
        #region inyecciónDependencias
        private readonly IRepository<Empleado> _empleadoRepo;

        public EmpleadoCreateModel(IRepository<Empleado> empleadoRepo) {
            _empleadoRepo = empleadoRepo;
        }
        #endregion

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [BindProperty]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [BindProperty]
        public string Ci { get; set; } = string.Empty;

        [BindProperty]
        public string ExtensionCi { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string DireccionDomicilio { get; set; } = string.Empty;

        [BindProperty]
        public string Telefono { get; set; } 

        [BindProperty]
        public DateOnly FechaNacimiento { get; set; }

        [BindProperty]
        public DateOnly FechaIngreso{ get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!EmpleadoValidator.esNombreValido(Nombre))
            {
                TempData["ErrorMessage"] = "El nombre no es válido (mínimo 2 caracteres y sin espacios a los lados).";
                return Page();
            }
            if (!EmpleadoValidator.esApellidoValido(ApellidoMaterno) || !EmpleadoValidator.esApellidoValido(ApellidoPaterno))
            {
                TempData["ErrorMessage"] = "El apellido no es válido (mínimo 4 caracteres)";
                return Page();
            }

            if (!EmpleadoValidator.esCiValido(Ci))
            {
                TempData["ErrorMessage"] = "El CI debe tener 8 dígitos";
                return Page();
            }

            if(!EmpleadoValidator.esExtensionCarnetValida(ExtensionCi))
            {
                TempData["ErrorMessage"] = "La extensión del carnet debe estar compuesta de un número y una letra.";
                return Page();
            }

            if (!EmpleadoValidator.esCorreoValido(Email))
            {
                TempData["ErrorMessage"] = "El formato del correo electrónico no es correcto.";
                return Page();
            }
            if(!EmpleadoValidator.esFechaNacimientoValida(FechaNacimiento))
            {
                TempData["ErrorMessage"] = "La fecha de nacimiento no es válida (debe ser al menos 18 años).";
                return Page();
            }
            if (!EmpleadoValidator.esTelefonoValido(Telefono))
            {
                TempData["ErrorMessage"] = "El número de teléfono debe tener 7-8 dígitos.";
                return Page();
            }
            if(!EmpleadoValidator.esDireccionValida(DireccionDomicilio))
            {
                TempData["ErrorMessage"] = "La dirección no es válida (mínimo 10 caracteres).";
                return Page();
            }
            if (!EmpleadoValidator.esFechaIngresoValida(FechaIngreso))
            {
                TempData["ErrorMessage"] = "La fecha de ingreso no es válida (no puede ser una fecha futura).";
                return Page();
            }
            Empleado empleado = new Empleado(Nombre, ApellidoPaterno, ApellidoMaterno, Ci, ExtensionCi, DireccionDomicilio, Email, Convert.ToInt32(Telefono), FechaNacimiento, FechaIngreso);

            if (_empleadoRepo.Insert(empleado) == 1)
            {
                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Error en La Base De Datos" });
            }
        }
    }
}