using LibreriaJoelito.FactoryCreators;
using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;
using LibreriaJoelito.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibreriaJoelito.Pages.Clientes
{
    public class CreateModel : PageModel
    {
        private readonly IRepository<Cliente> _clienteRepository;

        public CreateModel(IRepository<Cliente> clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        [BindProperty]
        public Cliente _cliente { get; set; } = new();

        public void OnGet()
        {
        }

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
                ModelState.AddModelError("_cliente.Ci", "Ya existe un cliente registrado con este CI y Complemento.");
                return Page();
            }

            _clienteRepository.Insert(_cliente);
            TempData["MensajeExito"] = $"Cliente '{_cliente.Nombre} {_cliente.ApellidoPaterno}' creado exitosamente.";

            return RedirectToPage("ClientesGet");
        }
    }
}
