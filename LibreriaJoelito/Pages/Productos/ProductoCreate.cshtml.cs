using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;
using LibreriaJoelito.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace LibreriaJoelito.Pages.Productos
{
    public class ProductoCreateModel : PageModel
    {
        private readonly IConfiguration configuration;

        [BindProperty]
        public string Categoria { get; set; }
        [BindProperty]
        public string Nombre { get; set; }
        [BindProperty]
        public decimal Precio{ get; set; }
        [BindProperty]
        public int Stock { get; set; }
        [TempData]
        public string MensajeExito { get; set; }

        public DataTable CategoriasDataTable { get; set; }
        public DataTable MarcasDataTable { get; set; }

        private readonly IRepository<Producto> _productRepository;
        public ProductoCreateModel(IConfiguration configuration, IRepository<Producto> productRepository)
        {
            this.configuration = configuration;
            _productRepository = productRepository;
        }
        public void OnGet()
        {
            LoadCategorias();
            LoadMarcas();
        }

        public IActionResult OnPost()
        {

            string query = @"INSERT INTO producto (Categoria, Nombre, Precio, Stock, Tipo_Venta, Factor_Conversion,Id_Producto_Base)
                            VALUES (@categoria, @nombre, @precio, @stock, @tipoVenta, @factorConversion, @idProductoBase);";
            MySqlCommand command = new MySqlCommand(query);

            command.Parameters.AddWithValue("@categoria", Categoria);
            command.Parameters.AddWithValue("@nombre", Nombre);
            command.Parameters.AddWithValue("@precio", Precio);
            command.Parameters.AddWithValue("@stock", Stock);
            command.Parameters.AddWithValue("@tipoVenta", TipoVenta);
            command.Parameters.AddWithValue("@factorConversion", FactorConversion);
            command.Parameters.AddWithValue("@idProductoBase", IdProductoBase);
            Producto producto= new Producto(Categoria,Nombre,Precio,Stock,TipoVenta,FactorConversion,IdProductoBase);

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
            _productRepository.Insert(producto);

            MensajeExito = "El producto fue creado correctamente.";

            return RedirectToPage("MostrarProductos");
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
        public class NombreSimple
        {
            public string Nombre { get; set; }
        }
        [ValidateAntiForgeryToken]
        public JsonResult OnPostCrearCategoria([FromBody] NombreSimple data)
        {
            Console.WriteLine("entro al post de crear categoria");
            data.Nombre= data.Nombre.Trim();
            if (string.IsNullOrWhiteSpace(data.Nombre)) 
            {
                return new JsonResult(new { ok = false, mensaje = "Nombre vacio" });
            }
            data.Nombre = data.Nombre.Trim();
            try
            {
                List<ValidationResult> errors = new List<ValidationResult>();
                errors = ExtraValidator.ValidarNombreCategoria(data.Nombre);
 
                if (errors.Any())
                {
                    Console.Write("hubo errores al validar nombre categoria");
                    return new JsonResult(new { success = false, message = errors.First().ErrorMessage });
                }
                string query = "INSERT INTO categoria (Nombre) VALUES (@nombre);";
                MySqlCommand cmd = new MySqlCommand( query);
                cmd.Parameters.AddWithValue("@nombre",data.Nombre);
                RepositorioBD.ExecuteNonQuery(cmd);
                LoadCategorias();
                return new JsonResult(new { ok = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ok = false, mensaje = ex.Message });
            }
        }
        [ValidateAntiForgeryToken]
        public JsonResult OnPostCrearMarca([FromBody] NombreSimple data)
        {
            if (string.IsNullOrWhiteSpace(data.Nombre))
            {
                return new JsonResult(new { ok = false, mensaje = "Nombre vacio" });
            }

            try
            {
                var errores = ExtraValidator.ValidarNombreMarca(data.Nombre);
                if (errores.Any())
                {
                    return new JsonResult(new { success = false, message = errores.First().ErrorMessage });
                }
                string query = "INSERT INTO marca (Nombre) VALUES (@nombre);";
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Parameters.AddWithValue("@nombre", data.Nombre);
                RepositorioBD.ExecuteNonQuery(cmd);
                LoadMarcas();
                return new JsonResult(new { ok = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { ok = false, mensaje = ex.Message });
            }
        }
    }
}
