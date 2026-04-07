using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.FactoryCreators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibreriaJoelito.Pages.Usuarios
{
    public class EmpleadoGetModel : PageModel
    {
        public DataTable EmpleadoDataTable { get; set; } = new DataTable();
        private readonly IConfiguration configuration;

        //Repository Inyectado Por dependencia
        private readonly IRepository<Usuario> _empleadoRepo;

        public EmpleadoGetModel(IRepository<Usuario> empleadoRepo)
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
        [BindProperty]
        public string Rol { get; set; }
        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Password { get; set; }
        



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
            Usuario empleado = new Usuario(Id);
            if (_empleadoRepo.Delete(empleado) == 1)
                TempData["SuccessMessage"] = "Empleado eliminado con éxito.";
            else
                TempData["ErrorMessage"] = "Hubo un problema al eliminar.";

            return RedirectToPage();
        }


        public IActionResult OnPostUpdate()
        {
            Usuario empleado = new Usuario(Id, Nombre, ApellidoPaterno, ApellidoMaterno, Ci, Complemento, DireccionDomicilio, Email, Telefono, FechaNacimiento, FechaIngreso,Username,Password,Rol);

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
