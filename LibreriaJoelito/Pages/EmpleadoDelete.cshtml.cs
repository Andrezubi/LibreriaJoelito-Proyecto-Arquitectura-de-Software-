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
                string connectionString = configuration.GetConnectionString("ConnectionMySql");
                string query = "DELETE FROM empleados WHERE Id = @id";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@id", Id);

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Empleado eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el empleado: " + ex.Message;
            }

            return Page(); 
        }
    }
}
