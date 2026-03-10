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
            if (!EmpleadoValidator.esNombreValido(Nombre))
            {
                TempData["ErrorMessage"] = "El nombre no es válido (mínimo 4 caracteres y sin espacios a los lados).";
                return Page();
            }

            if (!EmpleadoValidator.esCiValido(Ci))
            {
                TempData["ErrorMessage"] = "El CI debe tener más de 6 dígitos.";
                return Page();
            }

            if (!EmpleadoValidator.esCorreoValido(Email))
            {
                TempData["ErrorMessage"] = "El formato del correo electrónico no es correcto.";
                return Page();
            }

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