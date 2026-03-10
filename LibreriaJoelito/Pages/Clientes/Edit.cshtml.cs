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
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Cliente Cliente { get; set; } = new();

        public EditModel(IConfiguration configuration)
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
            AgregarErroresDeValidacion(ClienteValidator.Validar(Cliente));

            if (CIYaExisteEnOtroCliente(Cliente.CI, Cliente.Id))
                ModelState.AddModelError("Cliente.CI", "Ya existe otro cliente con ese CI.");

            if (!ModelState.IsValid)
                return Page();

            ActualizarCliente();

            TempData["MensajeExito"] = $"Cliente '{Cliente.Nombre} {Cliente.Apellido}' actualizado exitosamente.";
            return RedirectToPage("Index");
        }

        DataTable ObtenerClientePorId(int id)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionMySql")!;
            string query = @"SELECT Id, Nombre, Apellido, CI, Complemento, Email, EsClienteFrecuente
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
                Complemento = fila["Complemento"]?.ToString() ?? "",
                Email = fila["Email"] == DBNull.Value ? null : fila["Email"].ToString(),
                EsClienteFrecuente = Convert.ToBoolean(fila["EsClienteFrecuente"])
            };
        }

        void AgregarErroresDeValidacion(List<System.ComponentModel.DataAnnotations.ValidationResult> errores)
        {
            foreach (var error in errores)
                ModelState.AddModelError(error.MemberNames.First(), error.ErrorMessage!);
        }

        bool CIYaExisteEnOtroCliente(string ci, int idActual)
        {
            string connectionString = _configuration.GetConnectionString("ConnectionMySql")!;
            string query = "SELECT COUNT(*) FROM Clientes WHERE CI = @ci AND Complemento = @complemento AND Id <> @id";

            using MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();
            MySqlCommand comando = new MySqlCommand(query, conexion);
            comando.Parameters.AddWithValue("@ci", ci);
            comando.Parameters.AddWithValue("@complemento", Cliente.Complemento ?? "");
            comando.Parameters.AddWithValue("@id", idActual);
            return Convert.ToInt32(comando.ExecuteScalar()) > 0;
        }

        void ActualizarCliente()
        {
            string connectionString = _configuration.GetConnectionString("ConnectionMySql")!;
            string query = @"UPDATE Clientes SET
                                Nombre              = @nombre,
                                Apellido            = @apellido,
                                CI                  = @ci,
                                Complemento         = @complemento,
                                Email               = @email,
                                EsClienteFrecuente  = @frecuente,
                                UltimaActualizacion = NOW()
                             WHERE Id = @id";

            using MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();
            MySqlCommand comando = new MySqlCommand(query, conexion);
            comando.Parameters.AddWithValue("@nombre", Cliente.Nombre);
            comando.Parameters.AddWithValue("@apellido", Cliente.Apellido);
            comando.Parameters.AddWithValue("@ci", Cliente.CI);
            comando.Parameters.AddWithValue("@complemento", Cliente.Complemento ?? "");
            comando.Parameters.AddWithValue("@email", (object?)Cliente.Email ?? DBNull.Value);
            comando.Parameters.AddWithValue("@frecuente", Cliente.EsClienteFrecuente);
            comando.Parameters.AddWithValue("@id", Cliente.Id);
            comando.ExecuteNonQuery();
        }
    }
}
