using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Aplicacion.Interfaces;
using System.Linq; // Necesario para el .Select y .Any

namespace LibreriaJoelito.Pages.Marcas
{
    public class VerMarcasModel : PageModel
    {
        private readonly IRepository<Marca> _marcaRepo;
        private readonly MarcaValidator _marcaValidator;

        public VerMarcasModel(IRepository<Marca> marcaRepo, MarcaValidator marcaValidator)
        {
            _marcaRepo = marcaRepo;
            _marcaValidator = marcaValidator;
        }

        public DataTable MarcasDataTable { get; set; } = new DataTable();

        [BindProperty]
        public Marca Marca { get; set; } = new();

        public void OnGet()
        {
            MarcasDataTable = _marcaRepo.GetAll();
        }

        public JsonResult OnPostUpdate()
        {
            try
            {
                // Normalización de datos antes de validar
                Marca.Nombre = _marcaValidator.NormalizarTexto(Marca.Nombre);
                Marca.Industria = _marcaValidator.NormalizarTexto(Marca.Industria);

                var errores = _marcaValidator.Validar(Marca);
                var mensajesError = errores.Select(e => e.ErrorMessage).ToList();

                // Verificar duplicados
                if (_marcaRepo.ExisteDuplicado(Marca))
                {
                    mensajesError.Add("Ya existe una marca registrada con ese nombre.");
                }

                // Si hay 1 o más errores, los juntamos todos con un salto de línea (<br/>)
                if (mensajesError.Any())
                {
                    string errorAgrupado = string.Join("<br/>• ", mensajesError);
                    return new JsonResult(new { success = false, message = "• " + errorAgrupado });
                }

                // Si todo está bien, actualizamos
                int resultado = _marcaRepo.Update(Marca);

                if (resultado > 0)
                {
                    TempData["MensajeExito"] = $"Marca '{Marca.Nombre}' actualizada correctamente.";
                    return new JsonResult(new { success = true });
                }

                return new JsonResult(new { success = false, message = "No se realizaron cambios en la base de datos." });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error interno: " + ex.Message });
            }
        }

        public IActionResult OnPostDelete(int id)
        {
            _marcaRepo.Delete(new Marca(id));
            TempData["MensajeExito"] = "Marca eliminada correctamente.";
            return RedirectToPage();
        }
    }
}