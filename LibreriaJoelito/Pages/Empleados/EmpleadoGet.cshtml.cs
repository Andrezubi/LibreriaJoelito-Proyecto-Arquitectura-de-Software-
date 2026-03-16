using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using MySqlConnector;
using System.Data;

namespace LibreriaJoelito.Pages.Empleados
{
    public class EmpleadoGetModel : PageModel
    {
        public DataTable EmpleadoDataTable { get; set; } = new DataTable();
        private readonly IConfiguration configuration;

        public EmpleadoGetModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet()
        {
         Select();
        }

        public void Select()
        {
            string connectionString = configuration.GetConnectionString("ConnectionMySql")!;
            string query = @"SELECT Id,Nombre,ApellidoPaterno,ApellidoMaterno,Ci,Complemento,FechaNacimiento,Email,DireccionDomicilio,Telefono,FechaIngreso
                    FROM Empleado
                    WHERE estado = 1
                    ORDER BY 1;";
            MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(query);
            EmpleadoDataTable = RepositorioBD.ExecuteReturningDataTable(command);
        }

        public IActionResult OnPostDelete(int Id)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionMySql")!;
                string query = "UPDATE Empleado SET Estado = FALSE,FechaUltimaActualizacion = CURRENT_TIMESTAMP WHERE Id = @Id;";
                MySql.Data.MySqlClient.MySqlCommand command = new MySql.Data.MySqlClient.MySqlCommand(query);
                command.Parameters.AddWithValue("@id", Id);
                int filasAfectadas = RepositorioBD.ExecuteNonQuery(command);
                if (filasAfectadas > 0)
                    TempData["SuccessMessage"] = "Empleado eliminado exitosamente.";
                else
                    TempData["ErrorMessage"] = "No se pudo eliminar: el registro no existe.";

                return RedirectToPage("EmpleadoGet");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error crítico en la base de datos: " + ex.Message;
                return Page();
            }
        }

    }
}
