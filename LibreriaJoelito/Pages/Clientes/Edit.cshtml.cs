using LibreriaJoelito.FactoryCreators;
using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;
using LibreriaJoelito.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Pages.Clientes
{
    public class EditModel : PageModel
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
            _clienteRepository.Update(_cliente);
            TempData["MensajeExito"] = $"Cliente '{_cliente.Nombre} {_cliente.ApellidoPaterno}' Modificado.";

            return RedirectToPage("ClientesGet");
        }
    }
}
