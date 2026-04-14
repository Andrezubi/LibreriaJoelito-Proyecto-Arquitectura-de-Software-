using Google.Protobuf.WellKnownTypes;
using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Security.Claims;

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
        public Usuario usuario { get; set; } = new();

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            string tempPassword = _usuarioServicio.GenerarPassword(10);
            string tempUsername = _usuarioServicio.GenerarUsername(usuario.Nombre, usuario.ApellidoPaterno);
            usuario.IdUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            usuario.Password = tempPassword;
            usuario.Username = tempUsername;

   

            var result = _usuarioServicio.Insert(usuario);
      ;

            if (result.IsFailure)
            {
                foreach (var error in result.Errors)
                {
                    var parts = error.Split(':', 2);

                    if (parts.Length == 2)
                    {
                        var field = parts[0].Trim();
                        var message = parts[1].Trim();

                        ModelState.AddModelError(field, message);
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
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
                
                await _emailService.SendEmailAsync(usuario.Email, "Tus Credenciales - Librería Joelito", mensaje);
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
