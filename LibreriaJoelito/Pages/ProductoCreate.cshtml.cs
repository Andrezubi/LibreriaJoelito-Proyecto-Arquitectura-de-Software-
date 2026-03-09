using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Pages
{
    public class ProductoCreateModel : PageModel
    {
        private readonly IConfiguration configuration;

        [BindProperty]
        public string Categoria { get; set; }
        [BindProperty]
        public string Nombre { get; set; }
        [BindProperty]
        public decimal Precio { get; set; }
        [BindProperty]
        public int Stock { get; set; }
        [BindProperty]
        public string TipoVenta { get; set; }
        [BindProperty]
        public decimal FactorConversion { get; set; }

        public ProductoCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            string query = @"INSERT INTO productos (Categoria, Nombre, Precio, Stock, Tipo_Venta, Factor_Conversion)
                            VALUES (@categoria, @nombre, @precio, @stock, @tipoVenta, @factorConversion);";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@categoria", Categoria);
            command.Parameters.AddWithValue("@nombre", Nombre);
            command.Parameters.AddWithValue("@precio", Precio);
            command.Parameters.AddWithValue("@stock", Stock);
            command.Parameters.AddWithValue("@tipoVenta", TipoVenta);
            command.Parameters.AddWithValue("@factorConversion", FactorConversion);

            RepositorioBD.ExecuteNonQuery(command);

            return RedirectToPage("MostrarProductos");
        }
    }
}
