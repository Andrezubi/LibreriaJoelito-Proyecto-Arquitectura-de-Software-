using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibreriaJoelito.Pages.Marcas
{
    public class CreateMarcaModel : PageModel
    {
        private readonly IRepository<Marca> _marcaRepo;
        private readonly MarcaValidator _marcaValidator;

        public CreateMarcaModel(IRepository<Marca> marcaRepo, MarcaValidator marcaValidator)
        {
            _marcaRepo = marcaRepo;
            _marcaValidator = marcaValidator;
        }

        [BindProperty]
        public Marca Marca { get; set; } = new();

        public void OnGet()
        {
            // Solo carga la vista inicial
        }

        public IActionResult OnPost()
        {
            Marca.Nombre = _marcaValidator.NormalizarTexto(Marca.Nombre);
            Marca.Industria = _marcaValidator.NormalizarTexto(Marca.Industria);

            var errores = _marcaValidator.Validar(Marca);

            if (errores.Any())
            {
                foreach (var error in errores)
                {
                    ModelState.AddModelError(error.MemberNames.First(), error.ErrorMessage);
                }
                return Page();
            }

            if (_marcaRepo.ExisteDuplicado(Marca))
            {
                ModelState.AddModelError("Marca.Nombre", "Ya existe una marca registrada con este nombre.");
                return Page();
            }

            Marca.IdUsuario = 1; 

            if (_marcaRepo.Insert(Marca) > 0)
            {
                TempData["MensajeExito"] = $"Marca '{Marca.Nombre}' registrada exitosamente.";
                return RedirectToPage("VerMarcas");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al guardar en la base de datos.");
                return Page();
            }
        }
    }
}