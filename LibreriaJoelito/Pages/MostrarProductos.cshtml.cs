using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using LibreriaJoelito;
namespace LibreriaJoelito.Pages
{
    public class MostrarProductosModel : PageModel
    {
        public DataTable ProductosDataTable { get; set; } = new DataTable();
        private readonly IConfiguration configuration;

        public MostrarProductosModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet()
        {
            LoadProductos();

        }

        void LoadProductos()
        {
            string connectionString = configuration.GetConnectionString("MySqlConnection")!;
            string query = @"SELECT  Id, Nombre,Categoria,Precio,Stock,Tipo_Venta,Factor_Conversion,Id_Producto_Base, FechaRegistro
                            FROM productos
                            WHERE estado=1
                            ORDER BY 2";
            MySqlCommand command = new MySqlCommand(query);

            ProductosDataTable = RepositorioBD.ExecuteReturningDataTable(command);

        }
        public IActionResult OnPostDelete(int id)
        {
            string query = @"UPDATE productos
                     SET estado = 0
                     WHERE Id = @Id";

            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@Id", id);

            RepositorioBD.ExecuteNonQuery(cmd);

            return RedirectToPage("MostrarProductos");
        }

    }
}
