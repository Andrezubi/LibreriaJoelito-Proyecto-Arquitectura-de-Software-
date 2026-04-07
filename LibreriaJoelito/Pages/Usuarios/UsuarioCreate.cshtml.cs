using Google.Protobuf.WellKnownTypes;
using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages.Usuarios
{
    [Authorize(Roles = "Administrador")]
    public class EmpleadoCreateModel : PageModel
    {
        #region inyecciónDependencias
        private readonly UsuarioServicio _usuarioServicio;
        private readonly IEmailService _emailService;

        public EmpleadoCreateModel(UsuarioServicio usuarioServicio, IEmailService emailService)
        {
            _usuarioServicio = usuarioServicio;
            _emailService = emailService;
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

        public async Task<IActionResult> OnPostAsync()
        {
            string tempPassword = _usuarioServicio.GenerarPassword(10);
            string tempUsername = _usuarioServicio.GenerarUsername(Nombre, ApellidoPaterno);

            // 1 como IdUsuario creador (ajustar luego al usuario logueado)
            Usuario empleado = new Usuario(Nombre, ApellidoPaterno, ApellidoMaterno, Ci, ExtensionCi, DireccionDomicilio, Email, Telefono ?? "", FechaNacimiento, FechaIngreso, tempUsername, tempPassword, Rol, 1);

            var result = _usuarioServicio.Insert(empleado);

            if (result.IsFailure)
            {
                foreach (var error in result.Errors)
                {
                    Console.WriteLine(error);

                    ModelState.AddModelError(string.Empty, error);
                }
                return Page();
            }

            // --- INTEGRACIÓN HU-06: Envío de Email ---
            try 
            {
                string mensaje = $@"
                    <h3>¡Bienvenido a Librería Joelito!</h3>
                    <p>Tu cuenta ha sido creada exitosamente.</p>
                    <p><b>Usuario:</b> {tempUsername}</p>
                    <p><b>Contraseña Temporal:</b> {tempPassword}</p>
                    <p>Por favor, cambia tu contraseña al iniciar sesión por primera vez.</p>";
                
                await _emailService.SendEmailAsync(empleado.Email, "Tus Credenciales - Librería Joelito", mensaje);
            }
            catch (Exception ex)
            {
                // Loguear error pero no detener el flujo si el mail falla (opcional según requerimiento)
                Console.WriteLine("Error enviando email: " + ex.Message);
            }

            TempData["SuccessMessage"] = "Empleado creado exitosamente y credenciales enviadas por correo.";
            return RedirectToPage("UsuarioIndex");
        }
    }
}
