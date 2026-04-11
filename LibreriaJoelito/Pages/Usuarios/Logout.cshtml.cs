using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibreriaJoelito.Pages.Usuarios
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
        }
        public async Task<IActionResult> OnPost()
        {
            Console.WriteLine("ANTES LOGOUT: " + User.Identity?.Name);

            Response.Cookies.Delete("AuthToken", new CookieOptions
            {
                Path = "/"
            });

            Console.WriteLine("DESPUÉS LOGOUT: " + User.Identity?.Name);

            return RedirectToPage("/Index");
        }
    }
}
