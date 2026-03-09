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
        [BindProperty]
        public int? IdProductoBase { get; set; }
        public DataTable ProductosDataTable { get; set; }
        public ProductoCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet()
        {
            LoadProductos();
        }

        public IActionResult OnPost()
        {
            string query = @"INSERT INTO productos (Categoria, Nombre, Precio, Stock, Tipo_Venta, Factor_Conversion,Id_Producto_Base)
                            VALUES (@categoria, @nombre, @precio, @stock, @tipoVenta, @factorConversion, @idProductoBase);";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@categoria", Categoria);
            command.Parameters.AddWithValue("@nombre", Nombre);
            command.Parameters.AddWithValue("@precio", Precio);
            command.Parameters.AddWithValue("@stock", Stock);
            command.Parameters.AddWithValue("@tipoVenta", TipoVenta);
            command.Parameters.AddWithValue("@factorConversion", FactorConversion);
            command.Parameters.AddWithValue("@idProductoBase", IdProductoBase);

            RepositorioBD.ExecuteNonQuery(command);

            return RedirectToPage("MostrarProductos");
        }
        void LoadProductos()
        {
            string query = @"SELECT Id, Nombre 
                     FROM productos
                     WHERE estado = 1
                     ORDER BY Nombre";

            MySqlCommand cmd = new MySqlCommand(query);

            ProductosDataTable = RepositorioBD.ExecuteReturningDataTable(cmd);
        }
    }
}
