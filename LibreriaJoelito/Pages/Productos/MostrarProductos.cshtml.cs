using LibreriaJoelito.FactoryProducts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
namespace LibreriaJoelito.Pages.Productos
{
    public class MostrarProductosModel : PageModel
    {
        public DataTable ProductosDataTable { get; set; } = new DataTable();
        private readonly IConfiguration configuration;
        private ProductoRepository repository = new ProductoRepository();
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
            ProductosDataTable = repository.GetAll();

        }
        public IActionResult OnPostDelete(int id)
        {
            repository.Delete(new Models.Producto(id));
            return RedirectToPage("MostrarProductos");
        }
        
        public string getMarcaById(int? id)
        {
            if (id == 0) { return "ERROR no tiene Marca"; }
            string query = @"SELECT Nombre 
                            FROM marca
                            WHERE Id=@id";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@id", id);
            using (MySqlDataReader reader = RepositorioBD.ExecuteReader(cmd))
            {
                if (reader.Read())
                {
                    return reader["Nombre"].ToString()!;
                }
            }
            return "No se encontro La Marca";
        }
        public string getCategoriaById(int? id)
        {
            if (id == 0) { return "ERROR no tiene Categoria"; }
            string query = @"SELECT Nombre 
                            FROM categoria
                            WHERE Id=@id";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@id", id);
            using (MySqlDataReader reader = RepositorioBD.ExecuteReader(cmd))
            {
                if (reader.Read())
                {
                    return reader["Nombre"].ToString()!;
                }
            }
            return "No se encontro La Categoria";
        }

    }

}
