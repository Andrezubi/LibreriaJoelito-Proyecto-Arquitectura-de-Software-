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
            _clienteRepository.Insert(_cliente);
            TempData["MensajeExito"] = $"Cliente '{_cliente.Nombre} {_cliente.ApellidoPaterno}' creado.";

            return RedirectToPage("ClientesGet");
        }
    }
}
