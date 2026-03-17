using LibreriaJoelito.Models;
using LibreriaJoelito.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace LibreriaJoelito.Pages.Productos
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

        [BindProperty]
        public int? IdProductoBase { get; set; }

        public DataTable nombresProductosDataTable { get; set; }

        public ProductoUpdateModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet(byte id)
        {
            LoadProducto(id);
            LoadProductos(id);
        }

        public IActionResult LoadProducto(byte id)
        {
            string query = @"SELECT Categoria, Nombre, Precio, Stock, Tipo_Venta, Factor_Conversion,Id_Producto_Base
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
            IdProductoBase = row["Id_Producto_Base"] == DBNull.Value
                 ? null : Convert.ToInt32(row["Id_Producto_Base"]);


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
                                UltimaActualizacion = @fechaAhora,
                                Id_Producto_Base =@idProductoBase
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
            command.Parameters.AddWithValue("@idProductoBase", IdProductoBase);
            Producto producto = new Producto(Categoria, Nombre, Precio, Stock, TipoVenta, FactorConversion, IdProductoBase);
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
                LoadProductos(Id);
                return Page(); // vuelve al formulario mostrando errores
            }
            RepositorioBD.ExecuteNonQuery(command);

            return RedirectToPage("MostrarProductos");
        }
        void LoadProductos(int id)
        {
            string query = @"SELECT Id, Nombre 
                     FROM productos
                     WHERE estado = 1 And Id!=@id
                     ORDER BY Nombre";

            MySqlCommand cmd = new MySqlCommand(query);
            cmd.Parameters.AddWithValue("@id", id);

            nombresProductosDataTable = RepositorioBD.ExecuteReturningDataTable(cmd);
        }
    }
}
