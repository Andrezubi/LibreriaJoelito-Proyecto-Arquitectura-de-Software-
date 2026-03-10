using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages
{
    public class EmpleadoCreateModel : PageModel
    {
        private readonly IConfiguration configuration;

        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Apellidos { get; set; }

        [BindProperty]
        public string Ci { get; set; } 

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public DateOnly FechaNacimiento { get; set; }

        public EmpleadoCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionMySql")!;
                string query = @"INSERT INTO empleados 
                         (Nombre, Apellidos, CI, Email, Fecha_Nacimiento, Fecha_Ingreso, Estado) 
                         VALUES (@nombre, @apellidos, @ci, @email, @fechaNacimiento, CURDATE(), 1);";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@nombre", Nombre);
                    command.Parameters.AddWithValue("@apellidos", Apellidos);
                    command.Parameters.AddWithValue("@ci", Ci);
                    command.Parameters.AddWithValue("@email", Email);
                    command.Parameters.AddWithValue("@fechaNacimiento", FechaNacimiento.ToString("yyyy-MM-dd"));

                    connection.Open();
                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Empleado guardado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al guardar el empleado: " + ex.Message;
            }

            return RedirectToPage("EmpleadoCreate");
        }

    }
}
