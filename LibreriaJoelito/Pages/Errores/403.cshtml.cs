using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibreriaJoelito.Pages.Errores
{
    [AllowAnonymous]
    public class _403Model : PageModel
    {
        public void OnGet()
        {
        }
    }
}
