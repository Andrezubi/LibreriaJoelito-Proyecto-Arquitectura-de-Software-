using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages
{
    public class EmpleadoDeleteModel : PageModel
    {
        // Ya no necesitamos IConfiguration porque la conexión está dentro de RepositorioBD
        public EmpleadoDeleteModel()
        {
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
                string query = "DELETE FROM empleados WHERE Id = @id";
                MySqlCommand command = new MySqlCommand(query);
                command.Parameters.AddWithValue("@id", Id);
                int filasAfectadas = RepositorioBD.ExecuteNonQuery(command);

                if (filasAfectadas > 0)
                    TempData["SuccessMessage"] = "Empleado eliminado exitosamente.";
                else
                    TempData["ErrorMessage"] = "No se pudo eliminar: el registro no existe.";

                return RedirectToPage("/Index"); 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error crítico en la base de datos: " + ex.Message;
                return Page();
            }
        }
    }
}