using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages
{
    public class EmpleadoUpdateModel : PageModel
    {
        private readonly IConfiguration configuration;

        [BindProperty]
        public int Id { get; set; }

        [BindProperty]
        public string Nombre { get; set; }

        [BindProperty]
        public string Apellidos { get; set; }

        [BindProperty]
        public string Ci { get; set; }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public DateTime FechaNacimiento { get; set; }


        public EmpleadoUpdateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet(int id)
        {
            this.Id = id;
            Select();
        }

        public void Select()
        {
            string connectionString = configuration.GetConnectionString("ConnectionMySql")!;
            string query = @"SELECT id, Nombre, Apellidos, CI, Fecha_Nacimiento, Email, Fecha_Ingreso 
                            FROM empleados
                            WHERE id =  @id
                            ORDER BY 2;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", Id);
                connection.Open();

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read()) 
                    {
                        Nombre = reader["Nombre"].ToString()!;
                        Apellidos = reader["Apellidos"].ToString()!;
                        Ci = reader["CI"].ToString()!;
                        Email = reader["Email"]?.ToString() ?? "";
                        FechaNacimiento = Convert.ToDateTime(reader["Fecha_Nacimiento"]);
                    }
                }
            }
        }

        public IActionResult OnPost()
        {
            string connectionString = configuration.GetConnectionString("ConnectionMySql")!;

            string query = @"UPDATE empleados 
                     SET Nombre = @nombre, 
                         Apellidos = @apellidos, 
                         CI = @ci, 
                         Email = @email, 
                         Fecha_Nacimiento = @fechaNacimiento,
                         UltimaActualizacion = NOW()
                     WHERE id = @id;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                
                command.Parameters.AddWithValue("@nombre", Nombre);
                command.Parameters.AddWithValue("@apellidos", Apellidos);
                command.Parameters.AddWithValue("@ci", Ci);
                command.Parameters.AddWithValue("@email", Email);
                command.Parameters.AddWithValue("@fechaNacimiento", FechaNacimiento.ToString("yyyy-MM-dd"));

                command.Parameters.AddWithValue("@id", Id);

                connection.Open();
                command.ExecuteNonQuery();
            }


            return RedirectToPage("EmpleadoGet");
        }
    }
}
