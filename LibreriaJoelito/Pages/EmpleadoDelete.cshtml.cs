using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages
{
    public class EmpleadoDeleteModel : PageModel
    {
<<<<<<< HEAD
        // Ya no necesitamos IConfiguration porque la conexión está dentro de RepositorioBD
=======
>>>>>>> 7a379b204d95f11a9d6c752b945e3c3f3a61bc3a
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

<<<<<<< HEAD
                return RedirectToPage("/Index"); 
=======
                return RedirectToPage("/Index");
>>>>>>> 7a379b204d95f11a9d6c752b945e3c3f3a61bc3a
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error crítico en la base de datos: " + ex.Message;
                return Page();
            }
        }
    }
}