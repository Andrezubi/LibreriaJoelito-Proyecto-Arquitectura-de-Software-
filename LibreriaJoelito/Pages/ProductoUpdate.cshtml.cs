using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Pages
{
    public class ProductoUpdateModel : PageModel
    {
        private readonly IConfiguration configuration;

        [BindProperty]
        public byte Id { get; set; }

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

        public ProductoUpdateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet(byte id)
        {
            LoadProducto(id);
        }

        public IActionResult LoadProducto(byte id)
        {
            string query = @"SELECT Categoria, Nombre, Precio, Stock, Tipo_Venta, Factor_Conversion
                            FROM bdlibreria.productos
                            WHERE Id = @id;";
            MySqlCommand command = new MySqlCommand(query);
            command.Parameters.AddWithValue("@id", id);

            DataRow row = RepositorioBD.ExecuteReturningDataRow(command);

            Id = id;
            Categoria = row["Categoria"].ToString();
            Nombre = row["Nombre"].ToString();
            Precio = Convert.ToDecimal(row["Precio"]);
            Stock = Convert.ToInt32(row["Stock"]);
            TipoVenta = row["Tipo_Venta"].ToString();
            FactorConversion = Convert.ToDecimal(row["Factor_Conversion"]);


            return Page();
        }

        public IActionResult OnPost()
        {
            string query = @"UPDATE bdlibreria.productos
                            SET Categoria = @categoria,
	                            Nombre = @nombre,
                                Precio = @precio,
                                Stock = @stock,
                                Tipo_Venta = @tipoVenta,
                                Factor_Conversion = @factorConversion,
                                UltimaActualizacion = @fechaAhora
                            WHERE Id = @id;";

            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@categoria", Categoria);
            command.Parameters.AddWithValue("@nombre", Nombre);
            command.Parameters.AddWithValue("@precio", Precio);
            command.Parameters.AddWithValue("@stock", Stock);
            command.Parameters.AddWithValue("@tipoVenta", TipoVenta);
            command.Parameters.AddWithValue("@factorConversion", FactorConversion);
            command.Parameters.AddWithValue("@fechaAhora", DateTime.Now);
            command.Parameters.AddWithValue("@id", Id);

            RepositorioBD.ExecuteNonQuery(command);

            return RedirectToPage("MostrarProductos");
        }
    }
}
