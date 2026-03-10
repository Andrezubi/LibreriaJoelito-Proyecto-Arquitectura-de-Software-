using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages
{
    public class EmpleadoDeleteModel : PageModel
    {
        private readonly IConfiguration configuration;

        public EmpleadoDeleteModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [BindProperty]
        public int Id { get; set; }

        public IActionResult OnGet(int id)
        {
            Id = id;
            return Page();
        }

        public IActionResult OnPost()
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionMySql")!;
                string query = "DELETE FROM empleados WHERE Id = @id";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        // 1. PRIMERO agregas el parámetro
                        command.Parameters.AddWithValue("@id", Id);

                        int filasAfectadas = command.ExecuteNonQuery();

                        if (filasAfectadas > 0)
                            TempData["SuccessMessage"] = "Empleado eliminado exitosamente.";
                        else
                            TempData["ErrorMessage"] = "No se encontró el empleado.";
                    }
                }

                return RedirectToPage("/Empleados/Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar: " + ex.Message;
                return Page();
            }
        }
    }
}
