using LibreriaJoelito.Models;
using LibreriaJoelito.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace LibreriaJoelito.Pages.Productos
{
    public class ProductoCreateModel : PageModel
    {
        private readonly IConfiguration configuration;

        [BindProperty]
        public int IdCategoria { get; set; }
        [BindProperty]
        public string Nombre { get; set; }
        [BindProperty]
        public int IdMarca{ get; set; }
        [BindProperty]
        public int Stock { get; set; }
        
        [BindProperty]
        public int? IdProductoBase { get; set; }
        public DataTable ProductosDataTable { get; set; }
        public DataTable CategoriasDataTable { get; set; }
        public DataTable MarcasDataTable { get; set; }

        public ProductoCreateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void OnGet()
        {
            LoadCategorias();
            LoadMarcas();
        }

        public IActionResult OnPost()
        {
            Producto producto = new Producto(IdCategoria,IdMarca,Nombre,Stock);
            List<ValidationResult> errors = new List<ValidationResult>();
            errors=ProductValidator.ValidarProducto(producto); 
            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    foreach (var member in error.MemberNames)
                    {
                        ModelState.AddModelError(member, error.ErrorMessage);
                    }
                }
                LoadCategorias();
                LoadMarcas();
                return Page(); // vuelve al formulario mostrando errores
            }
            return RedirectToPage("MostrarProductos");
        }
        void LoadCategorias()
        {
            string query = @"SELECT Id, Nombre 
                     FROM categoria
                     WHERE estado = 1
                     ORDER BY Nombre";

            MySqlCommand cmd = new MySqlCommand(query);

            ProductosDataTable = RepositorioBD.ExecuteReturningDataTable(cmd);

        }
        void LoadMarcas()
        {
            string query = @"SELECT Id, Nombre 
                     FROM marca
                     WHERE estado = 1
                     ORDER BY Nombre";

            MySqlCommand cmd = new MySqlCommand(query);

            ProductosDataTable = RepositorioBD.ExecuteReturningDataTable(cmd);

        }
        public class NombreSimple
        {
            public string Nombre { get; set; }
        }
        public JsonResult OnPostCrearCategoria([FromBody] NombreSimple data)
        {
            if (string.IsNullOrWhiteSpace(data.Nombre))
            {
                return new JsonResult(new { ok = false, mensaje = "Nombre vacío" });
            }

            try
            {
                string query = "INSERT INTO categoria (Nombre) VALUES (@nombre)";
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Parameters.AddWithValue("@nombre", data.Nombre);
                RepositorioBD.ExecuteNonQuery(cmd);

                return new JsonResult(new { ok = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ok = false, mensaje = ex.Message });
            }
        }
        public JsonResult OnPostCrearMarca([FromBody] NombreSimple data)
        {
            if (string.IsNullOrWhiteSpace(data.Nombre))
            {
                return new JsonResult(new { ok = false, mensaje = "Nombre vacío" });
            }

            try
            {
                string query = "INSERT INTO marca (Nombre) VALUES (@nombre)";
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Parameters.AddWithValue("@nombre", data.Nombre);
                RepositorioBD.ExecuteNonQuery(cmd);

                return new JsonResult(new { ok = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ok = false, mensaje = ex.Message });
            }
        }
    }
}
