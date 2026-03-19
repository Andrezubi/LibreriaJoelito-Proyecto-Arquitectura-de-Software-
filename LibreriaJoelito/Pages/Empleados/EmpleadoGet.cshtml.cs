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
                TempData["SuccessMessage"] = "Empleado eliminado con éxito.";
            else
                TempData["ErrorMessage"] = "Hubo un problema al eliminar.";

            return RedirectToPage();
        }


        public IActionResult OnPostUpdate()
        {
            Empleado empleado = new Empleado(Id, Nombre, ApellidoPaterno, ApellidoMaterno, Ci, Complemento, DireccionDomicilio, Email, Telefono, FechaNacimiento, FechaIngreso);

            var resultados = EmpleadoValidator.Validar(empleado);

            if (resultados.Any())
            {
                return new JsonResult(new { succes= false, message = resultados.First().ErrorMessage });
            }
            

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
