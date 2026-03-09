using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;

namespace LibreriaJoelito.Pages.Clientes
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public string Nombre { get; set; }
        [BindProperty]
        public string Apellido { get; set; }
        [BindProperty]
        public string CI { get; set; }
        [BindProperty]
        public string Email { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            // Usando RepositorioBD para insertar
            MySqlCommand cmd = new MySqlCommand("INSERT INTO clientes (Nombre, Apellido, CI, Email) VALUES (@nombre, @apellido, @ci, @email)");
            cmd.Parameters.AddWithValue("@nombre", Nombre);
            cmd.Parameters.AddWithValue("@apellido", Apellido);
            cmd.Parameters.AddWithValue("@ci", CI);
            cmd.Parameters.AddWithValue("@email", Email);

            RepositorioBD.ExecuteNonQuery(cmd);

            return RedirectToPage("Index");
        }
    }
}
