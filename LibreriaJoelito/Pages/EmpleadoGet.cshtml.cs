using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Pages
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
            string query = @"SELECT id, Nombre, Apellidos, CI, Fecha_Nacimiento, Email, Fecha_Ingreso 
                    FROM empleados
                    WHERE estado = 1
                    ORDER BY 2;";
            MySqlCommand command = new MySqlCommand(query);
            EmpleadoDataTable = RepositorioBD.ExecuteReturningDataTable(command);
        }
    }
}
