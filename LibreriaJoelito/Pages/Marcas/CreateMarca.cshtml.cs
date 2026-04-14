using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace LibreriaJoelito.Pages.Marcas
{
    [Authorize(Roles = "Administrador,Empleado")]
    public class CreateMarcaModel : PageModel
    {
        private readonly MarcaServicio _marcaServicio;
        private readonly MarcaValidator _marcaValidator;

        public CreateMarcaModel(MarcaServicio marcaServicio, MarcaValidator marcaValidator)
        {
            _marcaServicio = marcaServicio;
            _marcaValidator = marcaValidator;
        }

        [BindProperty]
        public Marca Marca { get; set; } = new();

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            Marca.Nombre = _marcaValidator.NormalizarTexto(Marca.Nombre);
            Marca.Industria = _marcaValidator.NormalizarTexto(Marca.Industria);

            Marca.IdUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var result = _marcaServicio.Insert(Marca);

            if (result.IsFailure)
            {
                // Procesar errores devueltos por el servicio
                foreach (var error in result.Errors)
                {
                    var parts = error.Split(':', 2);
                    if (parts.Length == 2)
                    {
                        ModelState.AddModelError(parts[0].Trim(), parts[1].Trim());
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, error);
                    }
                }
                return Page();
            }

            TempData["MensajeExito"] = $"Marca '{Marca.Nombre}' registrada exitosamente.";
            return RedirectToPage("VerMarcas");
        }
    }
}