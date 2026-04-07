using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibreriaJoelito.Presentacion.Pages.Shared
{
    public class _LayoutModel : PageModel
    {
        public void OnGet()
        {
        }
        public IActionResult OnGetLogout()
        {
            Response.Cookies.Delete("AuthToken");
            return RedirectToPage("/LogIn/InicioSesion");
        }
    }
}
