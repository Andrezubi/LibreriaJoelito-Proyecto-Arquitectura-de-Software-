using LibreriaJoelito.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LibreriaJoelito.Pages.Empleados
{
    public class EmpleadoGetModel : PageModel
    {
        public DataTable EmpleadoDataTable { get; set; } = new DataTable();
        private readonly IConfiguration configuration;

        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Nombre { get; set; } = "";
        [BindProperty]
        public string ApellidoPaterno { get; set; } = "";
        [BindProperty]
        public string ApellidoMaterno { get; set; } = "";
        [BindProperty]
        public string Ci { get; set; } = "";
        [BindProperty]
        public string Complemento { get; set; } = "";
        [BindProperty]
        public string Email { get; set; } = "";
        [BindProperty]
        public string DireccionDomicilio { get; set; } = "";
        [BindProperty]
        public int Telefono { get; set; }
        [BindProperty]
        public DateTime FechaNacimiento { get; set; }
        [BindProperty]
        public DateTime FechaIngreso { get; set; }


        public EmpleadoGetModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet()
        {
            TempData.Clear();
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


        public IActionResult OnPostUpdate()
        {
            if (!EmpleadoValidator.esNombreValido(Nombre))
            {
                return new JsonResult(new { success = false, message = "El nombre no es válido (mínimo 2 caracteres y sin espacios a los lados)." });
            }

            if (!EmpleadoValidator.esApellidoValido(ApellidoPaterno))
            {
                return new JsonResult(new { success = false, message = "El apellido no es válido (mínimo 4 caracteres)." });
            }

            if (!EmpleadoValidator.esCiValido(Ci))
            {
                return new JsonResult(new { success = false, message = "El CI debe tener 8 dígitos." });
            }

            if (!EmpleadoValidator.esExtensionCarnetValida(Complemento))
            {
                return new JsonResult(new { success = false, message = "La extensión del carnet debe estar compuesta de un número y una letra." });
            }

            if (!EmpleadoValidator.esCorreoValido(Email))
            {
                return new JsonResult(new { success = false, message = "El formato del correo electrónico no es correcto." });
            }

            if (!EmpleadoValidator.esFechaNacimientoValidaUpdate(FechaNacimiento))
            {
                return new JsonResult(new { success = false, message = "La fecha de nacimiento no es válida (debe ser mayor de 18 años)." });
            }

            if (!EmpleadoValidator.esTelefonoValido(Telefono))
            {
                return new JsonResult(new { success = false, message = "El número de teléfono debe tener 7-8 dígitos." });
            }

            if (!EmpleadoValidator.esDireccionValida(DireccionDomicilio))
            {
                return new JsonResult(new { success = false, message = "La dirección no es válida (mínimo 10 caracteres)." });
            }

            if (!EmpleadoValidator.esFechaIngresoValidaUpdate(FechaIngreso))
            {
                return new JsonResult(new { success = false, message = "La fecha de ingreso no es válida (no puede ser una fecha futura)." });
            }


            string query = @"UPDATE empleado 
                     SET Nombre = @nombre, 
                         ApellidoPaterno = @apellidoPaterno, 
                         ApellidoMaterno = @apellidoMaterno, 
                         CI = @ci, 
                         Complemento = @complemento,
                         Email = @email, 
                         DireccionDomicilio = @direccion,
                         Telefono = @telefono,
                         FechaNacimiento = @fechaNacimiento,
                         FechaIngreso = @fechaIngreso,
                         FechaUltimaActualizacion = NOW() 
                     WHERE id = @id;";

            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@nombre", Nombre);
            command.Parameters.AddWithValue("@apellidoPaterno", ApellidoPaterno);
            command.Parameters.AddWithValue("@apellidoMaterno", ApellidoMaterno);
            command.Parameters.AddWithValue("@ci", Ci);
            command.Parameters.AddWithValue("@complemento", Complemento ?? "");
            command.Parameters.AddWithValue("@email", Email);
            command.Parameters.AddWithValue("@direccion", DireccionDomicilio);
            command.Parameters.AddWithValue("@telefono", Telefono);
            command.Parameters.AddWithValue("@fechaNacimiento", FechaNacimiento.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@fechaIngreso", FechaIngreso.ToString("yyyy-MM-dd"));
            command.Parameters.AddWithValue("@id", Id);

            RepositorioBD.ExecuteNonQuery(command);

            return new JsonResult(new { success = true });
        }

    }
}
