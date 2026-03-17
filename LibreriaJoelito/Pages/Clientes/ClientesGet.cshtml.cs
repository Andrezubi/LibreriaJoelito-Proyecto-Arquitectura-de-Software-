using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.FactoryCreators;
using LibreriaJoelito.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Pages.Clientes
{
    public class ClientesGetModel : PageModel
    {
        private readonly IRepository<Cliente> _clienteRepository = new ClienteRepositoryCreator().CreateRepository();
        public DataTable ClientesDataTable { get; set; } = new DataTable();
        public void OnGet()
        {
            ClientesDataTable = _clienteRepository.GetAll();
        }
    }
}
