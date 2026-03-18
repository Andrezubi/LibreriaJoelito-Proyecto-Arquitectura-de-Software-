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
            DataRow? fila = _clienteRepository.GetByID(id);
            if (fila is null)
            {
                return NotFound();
            }

            _cliente.Id = Convert.ToInt32(fila["Id"]);
            _cliente.Nombre = fila["Nombre"].ToString() ?? "";
            _cliente.ApellidoPaterno = fila["ApellidoPaterno"].ToString() ?? "";
            _cliente.ApellidoMaterno = fila["ApellidoMaterno"] == DBNull.Value ? null : fila["ApellidoMaterno"].ToString();
            _cliente.CI = fila["Ci"].ToString() ?? "";
            _cliente.Complemento = fila["Complemento"] == DBNull.Value ? null : fila["Complemento"].ToString();
            _cliente.Email = fila["Email"] == DBNull.Value ? null : fila["Email"].ToString();

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

       