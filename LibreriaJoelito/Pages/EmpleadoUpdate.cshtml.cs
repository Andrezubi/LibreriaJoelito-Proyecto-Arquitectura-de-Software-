using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages
{
    public class EmpleadoUpdateModel : PageModel
    {
        public EmpleadoUpdateModel() { }

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string Apellidos { get; set; } = string.Empty;

        [BindProperty]
        public string Ci { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public DateTime FechaNacimiento { get; set; }

        public void OnGet(int id)
        {
            this.Id = id;
            Select();
        }

        public void Select()
        {
            string query = @"SELECT Nombre, Apellidos, CI, Fecha_Nacimiento, Email 
                            FROM empleados
                            WHERE id = @id";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@id", Id);
            using (MySqlDataReader reader = RepositorioBD.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    Nombre = reader["Nombre"].ToString() ?? "";
                    Apellidos = reader["Apellidos"].ToString() ?? "";
                    Ci = reader["CI"].ToString() ?? "";
                    Email = reader["Email"]?.ToString() ?? "";
                    FechaNacimiento = Convert.ToDateTime(reader["Fecha_Nacimiento"]);
                }
            }
        }

        public IActionResult OnPost()
        {
            if (!EmpleadoValidator.esNombreValido(Nombre))
            {
                TempData["ErrorMessage"] = "El nombre no es válido (mínimo 4 caracteres).";
                return Page();
            }

            if (!EmpleadoValidator.esCiValido(Ci))
            {
                TempData["ErrorMessage"] = "El CI debe ser mayor a 6 dígitos.";
                return Page();
            }

            if (!EmpleadoValidator.esCorreoValido(Email))
            {
                TempData["ErrorMessage"] = "El correo electrónico no es válido.";
                return Page();
            }

            try
            {
                string query = @"UPDATE empleados 
                                 SET Nombre = @nombre, 
                                     Apellidos = @apellidos, 
                                     CI = @ci, 
                                     Email = @email, 
                                     Fecha_Nacimiento = @fechaNacimiento
                                 WHERE id = @id;";

                MySqlCommand command = new MySqlCommand(query);
                command.Parameters.AddWithValue("@nombre", Nombre);
                command.Parameters.AddWithValue("@apellidos", Apellidos);
                command.Parameters.AddWithValue("@ci", Ci);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@fechaNacimiento", FechaNacimiento.ToString("yyyy-MM-dd"));
                command.Parameters.AddWithValue("@id", Id);

                RepositorioBD.ExecuteNonQuery(command);

                TempData["SuccessMessage"] = "Empleado actualizado correctamente.";
                return RedirectToPage("EmpleadoGet");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar: " + ex.Message;
                return Page();
            }
        }
    }
}