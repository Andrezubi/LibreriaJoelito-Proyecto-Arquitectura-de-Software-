using LibreriaJoelito.FactoryCreators;
using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Pages.Clientes
{
    public class DeleteModel : PageModel
    {
        private readonly IRepository<Cliente> _clienteRepository = new ClienteRepositoryCreator().CreateRepository();
        [BindProperty]
        public Cliente _cliente { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            DataRow fila = _clienteRepository.GetByID(id);
            if (fila is null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            _clienteRepository.Delete(_cliente);
            TempData["MensajeExito"] = $"Cliente '{_cliente.Nombre} {_cliente.ApellidoPaterno}' Borrado.";

            return RedirectToPage("ClientesGet");
        }
    }
}

       