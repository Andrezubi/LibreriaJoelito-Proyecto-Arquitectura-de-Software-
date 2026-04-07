using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Aplicacion.Servicios;

namespace LibreriaJoelito.Pages.Marcas
{
    public class VerMarcasModel : PageModel
    {
        private readonly MarcaServicio _marcaServicio;
        private readonly MarcaValidator _marcaValidator;

        public VerMarcasModel(MarcaServicio marcaServicio, MarcaValidator marcaValidator)
        {
            _marcaServicio = marcaServicio;
            _marcaValidator = marcaValidator;
        }

        public DataTable MarcasDataTable { get; set; } = new DataTable();

        [BindProperty]
        public Marca Marca { get; set; } = new();

        public void OnGet()
        {
            MarcasDataTable = _marcaServicio.GetAll();
        }

        public JsonResult OnPostUpdate()
        {
            try
            {
                Marca.Nombre = _marcaValidator.NormalizarTexto(Marca.Nombre);
                Marca.Industria = _marcaValidator.NormalizarTexto(Marca.Industria);

                var result = _marcaServicio.Update(Marca);

                if (result.IsFailure)
                {
                    // Unimos todos los errores del Result para mostrarlos en el Modal
                    string errorAgrupado = string.Join("<br/>• ", result.Errors);
                    return new JsonResult(new { success = false, message = "• " + errorAgrupado });
                }

                TempData["MensajeExito"] = $"Marca '{Marca.Nombre}' actualizada correctamente.";
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error interno: " + ex.Message });
            }
        }

        public IActionResult OnPostDelete(int id)
        {
            Marca.Id = id;
            _marcaServicio.Delete(Marca);
            TempData["MensajeExito"] = "Marca eliminada correctamente.";
            return RedirectToPage();
        }
    }
}