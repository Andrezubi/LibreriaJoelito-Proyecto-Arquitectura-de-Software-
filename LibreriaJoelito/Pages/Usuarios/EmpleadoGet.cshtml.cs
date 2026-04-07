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

namespace LibreriaJoelito.Pages.Usuarios
{
    public class EmpleadoGetModel : PageModel
    {
        public DataTable EmpleadoDataTable { get; set; } = new DataTable();
        private readonly UsuarioServicio _usuarioServicio;

        public EmpleadoGetModel(UsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
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
            EmpleadoDataTable = _usuarioServicio.GetAll();
        }

        public IActionResult OnPostDelete(int Id)
        {
            Usuario usuario = new Usuario(Id);
            if (_usuarioServicio.Delete(usuario) == 1)
                TempData["SuccessMessage"] = "Empleado eliminado con éxito.";
            else
                TempData["ErrorMessage"] = "Hubo un problema al eliminar.";

            return RedirectToPage();
        }


        public IActionResult OnPostUpdate()
        {
            Usuario empleado = new Usuario(Id, Nombre, ApellidoPaterno, ApellidoMaterno, Ci, Complemento, DireccionDomicilio, Email, Telefono, FechaNacimiento, FechaIngreso,Username,Password,Rol);

            var result = _usuarioServicio.Update(empleado);

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
