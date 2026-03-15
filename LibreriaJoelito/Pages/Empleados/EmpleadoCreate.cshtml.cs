using Google.Protobuf.WellKnownTypes;
using LibreriaJoelito.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages.Empleados
{
    public class EmpleadoCreateModel : PageModel
    {
        public EmpleadoCreateModel() { }

        [BindProperty]
        public string Nombre { get; set; } = string.Empty;

        [BindProperty]
        public string ApellidoPaterno { get; set; } = string.Empty;

        [BindProperty]
        public string ApellidoMaterno { get; set; } = string.Empty;

        [BindProperty]
        public string Ci { get; set; } = string.Empty;

        [BindProperty]
        public string ExtensionCi { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string DireccionDomicilio { get; set; } = string.Empty;

        [BindProperty]
        public int Telefono { get; set; } 

        [BindProperty]
        public DateOnly FechaNacimiento { get; set; }

        [BindProperty]
        public DateOnly FechaIngreso{ get; set; }

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!EmpleadoValidator.esNombreValido(Nombre))
            {
                TempData["ErrorMessage"] = "El nombre no es válido (mínimo 2 caracteres y sin espacios a los lados).";
                return Page();
            }
            if (!EmpleadoValidator.esApellidoValido(ApellidoMaterno) || !EmpleadoValidator.esApellidoValido(ApellidoPaterno))
            {
                TempData["ErrorMessage"] = "El apellido no es válido (mínimo 4 caracteres)";
                return Page();
            }

            if (!EmpleadoValidator.esCiValido(Ci))
            {
                TempData["ErrorMessage"] = "El CI debe tener 8 dígitos";
                return Page();
            }

            if(!EmpleadoValidator.esExtensionCarnetValida(ExtensionCi))
            {
                TempData["ErrorMessage"] = "La extensión del carnet debe estar compuesta de un número y una letra.";
                return Page();
            }

            if (!EmpleadoValidator.esCorreoValido(Email))
            {
                TempData["ErrorMessage"] = "El formato del correo electrónico no es correcto.";
                return Page();
            }
            if(!EmpleadoValidator.esFechaNacimientoValida(FechaNacimiento))
            {
                TempData["ErrorMessage"] = "La fecha de nacimiento no es válida (debe ser al menos 18 años).";
                return Page();
            }
            if (!EmpleadoValidator.esTelefonoValido(Telefono))
            {
                TempData["ErrorMessage"] = "El número de teléfono debe tener 7-8 dígitos.";
                return Page();
            }
            if(!EmpleadoValidator.esDireccionValida(DireccionDomicilio))
            {
                TempData["ErrorMessage"] = "La dirección no es válida (mínimo 10 caracteres).";
                return Page();
            }
            if (!EmpleadoValidator.esFechaIngresoValida(FechaIngreso))
            {
                TempData["ErrorMessage"] = "La fecha de ingreso no es válida (no puede ser una fecha futura).";
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