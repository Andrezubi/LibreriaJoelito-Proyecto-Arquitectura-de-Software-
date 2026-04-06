using Google.Protobuf.WellKnownTypes;
using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace LibreriaJoelito.Pages.Empleados
{
    public class EmpleadoCreateModel : PageModel
    {
        #region inyecciónDependencias
        //private readonly IRepository<Empleado> _empleadoRepo;
        private readonly UsuarioServicio usuarioServicio;

        //public EmpleadoCreateModel(IRepository<Empleado> empleadoRepo) {
        //    _empleadoRepo = empleadoRepo;
        //}

        public EmpleadoCreateModel(UsuarioServicio usuarioServicio)
        {
            this.usuarioServicio = usuarioServicio;
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

        public void OnGet() { }

        public IActionResult OnPost()
        {
            
            Empleado empleado = new Empleado(Nombre, ApellidoPaterno, ApellidoMaterno, Ci, ExtensionCi, DireccionDomicilio, Email, Telefono, FechaNacimiento, FechaIngreso);

            //var resultados = EmpleadoValidator.Validar(empleado);
            var result = usuarioServicio.Insert(empleado);

            if (result.IsFailure)
            {
                TempData["CreateSucces"] = result.Errors.First();
                //foreach (var error in result.Errors)
                //{
                //    //Console.WriteLine(error);

                //    var parts = error.Split(':', 2);

                //    if (parts.Length == 2)
                //    {
                //        var field = parts[0].Trim();
                //        var message = parts[1].Trim();

                //        ModelState.AddModelError(field, message);
                //    }
                //    else
                //    {
                //        ModelState.AddModelError(string.Empty, error);
                //    }
                //}

                return Page();
            }

            TempData["SuccessMessage"] = "Empleado creado exitosamente.";

            return RedirectToPage("EmpleadoGet");

            //if (resultados.Any())
            //{
            //    TempData["CreateSucces"] = resultados.First().ErrorMessage;

            //    return Page();
            //}

            //if(_empleadoRepo.ExisteDuplicado(empleado))
            //{
            //    TempData["CreateSucces"] = "El empleado con ese CI ya existe";
            //    return Page();
            //}


            //if (_empleadoRepo.Insert(empleado) == 1)
            //{
            //    TempData["SuccessMessage"] = "Empleado creado exitosamente.";
            //    return RedirectToPage("EmpleadoGet");
            //}
            //else
            //{
            //    messageResult = "Error al registrar el empleado.";
            //    return Page();
            //}
        }
    }
}
