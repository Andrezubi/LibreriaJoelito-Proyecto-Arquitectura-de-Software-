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
        public string CI { get; set; } = string.Empty;
        [BindProperty]
        public string? Complemento { get; set; }
        [BindProperty]
        public string? Email { get; set; }
        [BindProperty]
        public bool EsclienteFrecuente { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            // Usando RepositorioBD para insertar
            MySqlCommand cmd = new MySqlCommand("INSERT INTO clientes (Nombre, Apellido, CI, Complemento, Email, EsClienteFrecuente) VALUES (@nombre, @apellido, @ci, @complemento, @email, @EsClienteFrecuente)");
            cmd.Parameters.AddWithValue("@nombre", Nombre);
            cmd.Parameters.AddWithValue("@apellido", Apellido);
            cmd.Parameters.AddWithValue("@ci", CI);
            cmd.Parameters.AddWithValue("@complemento", Complemento ?? string.Empty);
            cmd.Parameters.AddWithValue("@email", Email ?? string.Empty);
            cmd.Parameters.AddWithValue("@EsClienteFrecuente", EsclienteFrecuente);

            RepositorioBD.ExecuteNonQuery(cmd);

            return RedirectToPage("ClientesGet");
        }
    }
}
