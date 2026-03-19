using LibreriaJoelito.FactoryCreators;
using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;
using LibreriaJoelito.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibreriaJoelito.Pages.Empleados
{
    public class EmpleadoGetModel : PageModel
    {
        public DataTable EmpleadoDataTable { get; set; } = new DataTable();
        private readonly IConfiguration configuration;

        //Repository Inyectado Por dependencia
        private readonly IRepository<Empleado> _empleadoRepo;

        public EmpleadoGetModel(IRepository<Empleado> empleadoRepo)
        {
            _empleadoRepo = empleadoRepo;
        }


        public string messageResult { get; set; } = string.Empty;

        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Nombre { get; set; } = "";
        [BindProperty]
        public string ApellidoPaterno { get; set; } = "";
        [BindProperty]
        public string ApellidoMaterno { get; set; } = "";
        [BindProperty]
        public string Ci { get; set; } = "";
        [BindProperty]
        public string Complemento { get; set; } = "";
        [BindProperty]
        public string Email { get; set; } = "";
        [BindProperty]
        public string DireccionDomicilio { get; set; } = "";
        [BindProperty]
        public string Telefono { get; set; } = "";
        [BindProperty]
        public DateOnly FechaNacimiento { get; set; }
        [BindProperty]
        public DateOnly FechaIngreso { get; set; }


        public void OnGet()
        {
            TempData.Clear();
            Select();
        }

        public void Select()
        {
            EmpleadoDataTable = _empleadoRepo.GetAll();
        }

        public IActionResult OnPostDelete(int Id)
        {
            Empleado empleado = new Empleado(Id);
            if (_empleadoRepo.Delete(empleado) == 1)
            {
                messageResult = "Empleado eliminado exitosamente.";
                return Page();
            }
            else
            {
                messageResult = "Error al eliminar el empleado.";
                return Page();
            }
        }


        public IActionResult OnPostUpdate()
        {
            if (!EmpleadoValidator.esNombreValido(Nombre))
            {
                return new JsonResult(new { success = false, message = "El nombre no es válido (mínimo 2 caracteres y sin espacios a los lados)." });
            }

            if (!EmpleadoValidator.esApellidoValido(ApellidoPaterno))
            {
                return new JsonResult(new { success = false, message = "El apellido no es válido (mínimo 4 caracteres)." });
            }

            if (!EmpleadoValidator.esCiValido(Ci))
            {
                return new JsonResult(new { success = false, message = "El CI debe tener 8 dígitos." });
            }

            if (!EmpleadoValidator.esExtensionCarnetValida(Complemento))
            {
                return new JsonResult(new { success = false, message = "La extensión del carnet debe estar compuesta de un número y una letra." });
            }

            if (!EmpleadoValidator.esCorreoValido(Email))
            {
                return new JsonResult(new { success = false, message = "El formato del correo electrónico no es correcto." });
            }

            if (!EmpleadoValidator.esFechaNacimientoValida(FechaNacimiento))
            {
                return new JsonResult(new { success = false, message = "La fecha de nacimiento no es válida (debe ser mayor de 18 años)." });
            }

            if (!EmpleadoValidator.esTelefonoValido(Telefono))
            {
                return new JsonResult(new { success = false, message = "El número de teléfono debe tener 7-8 dígitos." });
            }

            if (!EmpleadoValidator.esDireccionValida(DireccionDomicilio))
            {
                return new JsonResult(new { success = false, message = "La dirección no es válida (mínimo 10 caracteres)." });
            }

            if (!EmpleadoValidator.esFechaIngresoValida(FechaIngreso))
            {
                return new JsonResult(new { success = false, message = "La fecha de ingreso no es válida (no puede ser una fecha futura)." });
            }


            Empleado empleado = new Empleado(Id, Nombre, ApellidoPaterno, ApellidoMaterno, Ci, Complemento, DireccionDomicilio, Email, Convert.ToInt32(Telefono), FechaNacimiento, FechaIngreso);

            if (_empleadoRepo.Update(empleado) == 1){
                return new JsonResult(new { success = true });
            }
            else
            {
                return new JsonResult(new { success = false, message = "Error en La Base De Datos" });
            }

        }

    }
}
