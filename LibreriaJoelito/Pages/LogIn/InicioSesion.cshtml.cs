using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibreriaJoelito.Aplicacion.Interfaces;

namespace LibreriaJoelito.Pages.LogIn
{
    public class InicioSesionModel : PageModel
    {
        private readonly IServicioUsuario _servicioUsuario;

        public InicioSesionModel(IServicioUsuario servicioUsuario)
        {
            _servicioUsuario = servicioUsuario;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var result = _servicioUsuario.Login(Username, Password);

            if (result.Success)
            {
                // luego puedes guardar sesión o JWT
                return RedirectToPage("/Index");
            }

            ErrorMessage = result.Message;
            return Page();
        }
    }
}