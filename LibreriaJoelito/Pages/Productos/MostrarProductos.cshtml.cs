using LibreriaJoelito.FactoryProducts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using LibreriaJoelito.Models;
namespace LibreriaJoelito.Pages.Productos
{
    public class MostrarProductosModel : PageModel
    {
        public DataTable ProductosDataTable { get; set; } = new DataTable();

        public DataTable CategoriasDataTable { get; set; }
        public DataTable MarcasDataTable { get; set; }

        [BindProperty]
        public int Id { get; set; }
        [BindProperty]
        public string Nombre { get; set; }
        [BindProperty]
        public int IdCategoria { get; set; }
        [BindProperty]
        public int IdMarca { get; set; }
        [BindProperty]
        public int Stock { get; set; }




        private readonly IConfiguration configuration;
        private readonly IRepository<Producto> _productRepository;
        public MostrarProductosModel(IConfiguration configuration,IRepository<Producto> productoRepo)
        {
            this.configuration = configuration;
            this._productRepository = productoRepo;
        }
        public void OnGet()
        {
           LoadProductos();
           LoadCategorias();
           LoadMarcas();
        }

        void LoadProductos()
        {
            ProductosDataTable = _productRepository.GetAll();

        }

        void LoadCategorias()
        {
            string query = @"SELECT Id, Nombre 
                     FROM categoria
                     WHERE estado = 1
                     ORDER BY Nombre";

            MySqlCommand cmd = new MySqlCommand(query);

            CategoriasDataTable = RepositorioBD.ExecuteReturningDataTable(cmd);

        }
        void LoadMarcas()
        {
            string query = @"SELECT Id, Nombre 
                     FROM marca
                     WHERE estado = 1
                     ORDER BY Nombre";

            MySqlCommand cmd = new MySqlCommand(query);

            MarcasDataTable = RepositorioBD.ExecuteReturningDataTable(cmd);

        }

        public IActionResult OnPostDelete(int id)
        {
            _productRepository.Delete(new Models.Producto(id));
            return RedirectToPage("MostrarProductos");
        }

        public IActionResult OnPostUpdate()
        {
            Producto producto = new Producto(Id, IdCategoria, IdMarca, Nombre, Stock);
            _productRepository.Update(producto);
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
