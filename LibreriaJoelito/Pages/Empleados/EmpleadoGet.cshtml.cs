using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.FactoryCreators;
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
        private readonly UsuarioServicio usuarioServicio;

        public EmpleadoGetModel(UsuarioServicio usuarioServicio)
        {
            this.usuarioServicio = usuarioServicio;
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
            EmpleadoDataTable = usuarioServicio.GetAll();
        }

        public IActionResult OnPostDelete(int Id)
        {
            Empleado empleado = new Empleado(Id);
            if (usuarioServicio.Delete(empleado) == 1)
                TempData["SuccessMessage"] = "Empleado eliminado con éxito.";
            else
                TempData["ErrorMessage"] = "Hubo un problema al eliminar.";

            return RedirectToPage();
        }


        public IActionResult OnPostUpdate()
        {
            Empleado empleado = new Empleado(Id, Nombre, ApellidoPaterno, ApellidoMaterno, Ci, Complemento, DireccionDomicilio, Email, Telefono, FechaNacimiento, FechaIngreso);

            var result = usuarioServicio.Update(empleado);

            if (result.IsFailure)
            {
                return new JsonResult(
                    new
                    {
                        success = false,
                        message = result.Errors.First()
                    }
                );
            }

            return new JsonResult(new { success = true });
        }

    }
}
