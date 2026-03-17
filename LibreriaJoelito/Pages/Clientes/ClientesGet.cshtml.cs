using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Pages.Clientes
{
    public class ClientesGetModel : PageModel
    {
        public DataTable ClientesDataTable { get; set; } = new DataTable();

        public void OnGet()
        {
            // Adaptado para usar RepositorioBD existente
           MySqlCommand cmd = new MySqlCommand("SELECT Id, Nombre, Apellido, CI, Complemento, Email, EsClienteFrecuente, FechaRegistro FROM clientes WHERE Estado = 1 ORDER BY Apellido, Nombre");
            ClientesDataTable = RepositorioBD.ExecuteReturningDataTable(cmd);
        }
    }
}
