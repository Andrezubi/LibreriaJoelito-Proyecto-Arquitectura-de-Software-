using LibreriaJoelito.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Pages.Clientes
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Cliente Cliente { get; set; } = new();

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet(int id)
        {
            var tabla = ObtenerClientePorId(id);

            if (tabla.Rows.Count == 0)
                return NotFound();

            CargarClienteDesdeTabla(tabla.Rows[0]);
            return Page();
        }

        public IActionResult OnPost()
        {
            DeshabilitarCliente(Cliente.Id);
            TempData["MensajeExito"] = $"Cliente '{Cliente.Nombre} {Cliente.Apellido}' eliminado.";
            return RedirectToPage("Index");
        }

        DataTable ObtenerClientePorId(int id)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionMySql")!;
            string query = @"SELECT Id, Nombre, Apellido, CI, Email, EsClienteFrecuente, FechaRegistro
                             FROM Clientes
                             WHERE Id = @id AND Estado = 1";

            using MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();
            MySqlCommand comando = new MySqlCommand(query, conexion);
            comando.Parameters.AddWithValue("@id", id);
            MySqlDataAdapter adaptador = new MySqlDataAdapter(comando);
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            return tabla;
        }

        void CargarClienteDesdeTabla(DataRow fila)
        {
            Cliente = new Cliente
            {
                Id = Convert.ToInt32(fila["Id"]),
                Nombre = fila["Nombre"].ToString()!,
                Apellido = fila["Apellido"].ToString()!,
                CI = fila["CI"].ToString()!,
                Email = fila["Email"] == DBNull.Value ? null : fila["Email"].ToString(),
                EsClienteFrecuente = Convert.ToBoolean(fila["EsClienteFrecuente"]),
                FechaRegistro = Convert.ToDateTime(fila["FechaRegistro"])
            };
        }

        void DeshabilitarCliente(int id)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionMySql")!;
            string query = "UPDATE Clientes SET Estado = 0, UltimaActualizacion = NOW() WHERE Id = @id";

            using MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();
            MySqlCommand comando = new MySqlCommand(query, conexion);
            comando.Parameters.AddWithValue("@id", id);
            comando.ExecuteNonQuery();
        }
    }
}
