using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibreriaJoelito.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly Aplicacion.Interfaces.IEmailService _emailService;

        public IndexModel(ILogger<IndexModel> logger, Aplicacion.Interfaces.IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public async Task OnGet()
        {
            // PRUEBA DE ENVÍO (Descomenta para probar después de configurar appsettings.json)
            // await _emailService.SendEmailAsync("tu-correo@example.com", "Prueba Librería Joelito", "Este es un correo de prueba.");
        }
    }
}
