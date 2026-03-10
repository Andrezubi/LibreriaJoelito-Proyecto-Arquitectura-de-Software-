using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages
{
    public class EmpleadoCreateModel : PageModel
    {
        public EmpleadoCreateModel() { }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Apellidos { get; set; } = string.Empty;

        [BindProperty]
        public string Ci { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public DateOnly FechaNacimiento { get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            try
            {
                string query = @"INSERT INTO empleados 
                         (Nombre, Apellidos, CI, Email, Fecha_Nacimiento, Fecha_Ingreso, Estado) 
                         VALUES (@nombre, @apellidos, @ci, @email, @fechaNacimiento, CURDATE(), 1);";
                MySqlCommand command = new MySqlCommand(query);
                command.Parameters.AddWithValue("@nombre", Nombre);
                command.Parameters.AddWithValue("@apellidos", Apellidos);
                command.Parameters.AddWithValue("@ci", Ci);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@fechaNacimiento", FechaNacimiento.ToString("yyyy-MM-dd"));
                RepositorioBD.ExecuteNonQuery(command);

                TempData["SuccessMessage"] = "Empleado guardado exitosamente.";
                return RedirectToPage("EmpleadoGet");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al guardar: " + ex.Message;
                return Page();
            }
        }
    }
}