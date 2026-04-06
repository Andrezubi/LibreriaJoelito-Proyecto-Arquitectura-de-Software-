using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.Persistencia;
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
        public RepositorioBD bd { get; set; } = RepositorioBD.Instancia;

        [BindProperty]
        public int IdCategoria { get; set; }
        [BindProperty]
        public string Nombre { get; set; }
        [BindProperty]
        public int IdMarca { get; set; }
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
            Producto producto = new Producto(IdCategoria, IdMarca, Nombre, Stock);
            List<ValidationResult> errors = new List<ValidationResult>();
            errors = ProductValidator.ValidarProducto(producto);
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

            CategoriasDataTable = bd.ExecuteReturningDataTable(cmd);

        }
        void LoadMarcas()
        {
            string query = @"SELECT Id, Nombre 
                     FROM marca
                     WHERE estado = 1
                     ORDER BY Nombre";

            MySqlCommand cmd = new MySqlCommand(query);

            MarcasDataTable = bd.ExecuteReturningDataTable(cmd);

        }
        public class NombreSimple
        {
            public string Nombre { get; set; }
        }
        [ValidateAntiForgeryToken]
        public JsonResult OnPostCrearCategoria([FromBody] NombreSimple data)
        {
            Console.WriteLine("entro al post de crear categoria");
            data.Nombre = data.Nombre.Trim();
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
                MySqlCommand cmd = new MySqlCommand(query);
                cmd.Parameters.AddWithValue("@nombre", data.Nombre);
                bd.ExecuteNonQuery(cmd);
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
                var bd = RepositorioBD.Instancia;
                bd.ExecuteNonQuery(cmd);
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