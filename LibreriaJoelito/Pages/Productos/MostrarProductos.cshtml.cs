using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.Persistencia;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace LibreriaJoelito.Pages.Productos
{
    public class MostrarProductosModel : PageModel
    {
        public DataTable ProductosDataTable { get; set; } = new DataTable();

        public DataTable CategoriasDataTable { get; set; }
        public DataTable MarcasDataTable { get; set; }
        public RepositorioBD bd { get; set; } = RepositorioBD.Instancia;

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
        [TempData]
        public string MensajeExito { get; set; }


        private readonly IConfiguration configuration;
        private readonly ProductoServicio productoServicio;

        public MostrarProductosModel(IConfiguration configuration, ProductoServicio productoServicio)
        {
            this.configuration = configuration;
            this.productoServicio = productoServicio;
        }

        public void OnGet()
        {
           LoadProductos();
           LoadCategorias();
           LoadMarcas();
        }

        void LoadProductos()
        {
            ProductosDataTable = productoServicio.GetAll();

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

        public IActionResult OnPostDelete(int id)
        {
            productoServicio.Delete(new Dominio.Models.Producto(id));
            TempData["MensajeExito"] = "El producto fue eliminado correctamente.";
            return RedirectToPage("MostrarProductos");
        }

        //public IActionResult OnPostUpdate()
        //{
        //    Producto producto = new Producto(Id, IdCategoria, IdMarca, Nombre, Stock);
        //    var errores =ProductValidator.ValidarProducto(producto);
        //    if (errores.Any())
        //    {
        //        return new JsonResult(new { ok = false, message = errores.First().ErrorMessage });
        //    }
        //    _productRepository.Update(producto);
        //    TempData["MensajeExito"] = "El producto fue editado correctamente.";
        //    return RedirectToPage("MostrarProductos");
        //}
        public JsonResult OnPostUpdate()
        {
            try
            {
                Producto producto = new Producto(Id, IdCategoria, IdMarca, Nombre, Stock);
                //var errores = ProductValidator.ValidarProducto(producto);
                var result = productoServicio.Update(producto);

                // validar (tu l�gica actual)
                //if (errores.Any())
                //{
                //    var listaErrores = errores
                //        .Select(e => e.ErrorMessage)
                //        .ToList();

                //    return new JsonResult(new { ok = false, errores = listaErrores });
                //}
                //_productRepository.Update(producto);

                if (result.IsFailure)
                {
                    return new JsonResult(
                        new
                        {
                            success = false,
                            message = result.Errors
                        }
                    );
                }

                TempData["MensajeExito"] = "El producto fue editado correctamente.";
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }

        public string getMarcaById(int? id)
        {
            if (id == 0) { return "ERROR no tiene Marca"; }
            string query = @"SELECT Nombre 
                            FROM marca
                            WHERE Id=@id";
            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@id", id);
            using (MySqlDataReader reader = bd.ExecuteReader(cmd))
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
            using (MySqlDataReader reader = bd.ExecuteReader(cmd))
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
