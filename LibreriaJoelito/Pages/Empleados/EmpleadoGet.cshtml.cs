using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
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
            MySqlCommand command = new MySqlCommand(query);
            EmpleadoDataTable = RepositorioBD.ExecuteReturningDataTable(command);
        }
    }
}
