using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.FactoryCreators;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibreriaJoelito.Pages.Clientes
{
    public class CreateModel : PageModel
    {
        private readonly ClienteServicio clienteServicio;
        private readonly ClienteValidator clienteValidator;

        public CreateModel(ClienteServicio clienteServicio, ClienteValidator clienteValidator)
        {
            this.clienteServicio = clienteServicio;
            this.clienteValidator = clienteValidator;
        }

        [BindProperty]
        public Cliente _cliente { get; set; } = new();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Normalización
            _cliente.Nombre = clienteValidator.NormalizarTexto(_cliente.Nombre);
            _cliente.ApellidoPaterno = clienteValidator.NormalizarTexto(_cliente.ApellidoPaterno);
            _cliente.ApellidoMaterno = clienteValidator.NormalizarTexto(_cliente.ApellidoMaterno);

            var result = clienteServicio.Insert(_cliente);

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

            TempData["MensajeExito"] =
                $"Cliente '{_cliente.Nombre} {_cliente.ApellidoPaterno}' creado exitosamente.";

            return RedirectToPage("ClientesGet");
        }
    }
}
