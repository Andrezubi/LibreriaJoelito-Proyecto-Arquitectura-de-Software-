using LibreriaJoelito.FactoryCreators;
using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages.Clientes
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<Cliente> _clienteRepository 
            = new ClienteRepositoryCreator().CreateRepository();

        [BindProperty]
        public Cliente _cliente { get; set; } = new();

        public void OnGet(){}

        public IActionResult OnPost()
        {
            // Normalización
            _cliente.Nombre = ClienteValidator.NormalizarTexto(_cliente.Nombre);
            _cliente.ApellidoPaterno = ClienteValidator.NormalizarTexto(_cliente.ApellidoPaterno);
            _cliente.ApellidoMaterno = ClienteValidator.NormalizarTexto(_cliente.ApellidoMaterno);

            // Validación
            var errores = ClienteValidator.Validar(_cliente);
            if (errores.Any())
            {
                foreach (var err in errores)
                {
                    ModelState.AddModelError(err.MemberNames.First(), err.ErrorMessage ?? "Error");
                }
                return Page();
            }

            // Verificar Duplicados (CI + Complemento)
            if (_clienteRepository is ClienteRepository repo && repo.ExisteDuplicado(_cliente))
            {
                ModelState.AddModelError("Cliente.CI", "Ya existe un cliente registrado con este CI y Complemento.");
                return Page();
            }

            _clienteRepository.Insert(_cliente);
            TempData["MensajeExito"] = $"Cliente '{_cliente.Nombre} {_cliente.ApellidoPaterno}' creado exitosamente.";

            return RedirectToPage("ClientesGet");
        }
    }
}
