using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.FactoryCreators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibreriaJoelito.Pages.Usuarios
{
    [Authorize(Roles = "Administrador")]
    public class EmpleadoGetModel : PageModel
    {
        public DataTable EmpleadoDataTable { get; set; } = new DataTable();
        private readonly UsuarioServicio _usuarioServicio;

        public EmpleadoGetModel(UsuarioServicio usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }


        public string messageResult { get; set; } = string.Empty;

        //[BindProperty]
        //public int Id { get; set; }
        //[BindProperty]
        //public string Nombre { get; set; } = "";
        //[BindProperty]
        //public string ApellidoPaterno { get; set; } = "";
        //[BindProperty]
        //public string ApellidoMaterno { get; set; } = "";
        //[BindProperty]
        //public string Ci { get; set; } = "";
        //[BindProperty]
        //public string Complemento { get; set; } = "";
        //[BindProperty]
        //public string Email { get; set; } = "";
        //[BindProperty]
        //public string DireccionDomicilio { get; set; } = "";
        //[BindProperty]
        //public string Telefono { get; set; } = "";
        //[BindProperty]
        //public DateOnly FechaNacimiento { get; set; }
        //[BindProperty]
        //public DateOnly FechaIngreso { get; set; }
        //[BindProperty]
        //public string Rol { get; set; }
        //[BindProperty]
        //public string Username { get; set; }
        //[BindProperty]
        //public string Password { get; set; }

        [BindProperty]
        public Usuario usuario { get; set; } = new();




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
            usuario.Id = Id;
            usuario.IdUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if (_usuarioServicio.Delete(usuario) == 1)
                TempData["SuccessMessage"] = "Empleado eliminado con éxito.";
            else
                TempData["ErrorMessage"] = "Hubo un problema al eliminar.";

            return RedirectToPage();
        }


        public IActionResult OnPostUpdate()
        {

            usuario.IdUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);    

            var result = _usuarioServicio.Update(usuario);

            if (result.IsFailure)
            {
                return new JsonResult(new { success = false, message = result.Errors.First() });
            }
            

            return new JsonResult(new { success = true });
        }

    }
}
