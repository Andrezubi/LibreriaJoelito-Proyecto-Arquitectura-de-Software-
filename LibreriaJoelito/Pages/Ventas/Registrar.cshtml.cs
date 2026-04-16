using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;

namespace LibreriaJoelito.Pages.Ventas
{
    public class RegistrarModel : PageModel
    {
        private readonly ClienteServicio _clienteServicio;
        private readonly ProductoRepository _productoRepository = new ProductoRepository();

        public RegistrarModel(ClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        public void OnGet() { }
        public JsonResult OnGetBuscarCliente(string ci)
        {
            if (string.IsNullOrWhiteSpace(ci))
            {
                return new JsonResult(new { success = false, message = "CI no proporcionado" });
            }

            var cliente = _clienteServicio.BuscarPorCi(ci);

            if (cliente != null)
            {
                return new JsonResult(new
                {
                    success = true,
                    cliente = new
                    {
                        cliente.Id,
                        cliente.Nombre,
                        cliente.ApellidoPaterno,
                        cliente.ApellidoMaterno
                    }
                });
            }

            return new JsonResult(new { success = false, message = "Cliente no encontrado" });
        }

        [ValidateAntiForgeryToken]
        public JsonResult OnPostCrearCliente([FromBody] Cliente cliente)
        {
            if (cliente == null)
            {
                return new JsonResult(new { success = false, message = "Datos inválidos" });
            }

            cliente.Estado = true;
            cliente.FechaRegistro = DateTime.Now;

            cliente.IdUsuario = 1;

            var result = _clienteServicio.Insert(cliente);

            if (!result.IsSuccess)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = string.Join(", ", result.Errors)
                });
            }

            var nuevo = _clienteServicio.BuscarPorCi(cliente.Ci);

            return new JsonResult(new
            {
                success = true,
                cliente = new
                {
                    nuevo.Id,
                    nuevo.Nombre,
                    nuevo.ApellidoPaterno,
                    nuevo.ApellidoMaterno,
                    nuevo.Ci
                }
            });
        }
        public JsonResult OnGetBuscarNombre(string termino)
        {
            DataTable dt = _productoRepository.BuscarPorNombre(termino);
            var listaNombres = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                listaNombres.Add(row["Nombre"].ToString());
            }

            return new JsonResult(listaNombres);
        }
        public IActionResult OnGetObtenerDetalleProducto(string nombre)
        {
            if (string.IsNullOrEmpty(nombre))
            {
                return new JsonResult(new { success = false, message = "El nombre está vacío." });
            }
            DataTable dtProducto = _productoRepository.BuscarProducto(nombre);

            if (dtProducto.Rows.Count > 0)
            {
                DataRow row = dtProducto.Rows[0];
                return new JsonResult(new
                {
                    success = true,
                    producto = new
                    {
                        id = Convert.ToInt32(row["Id"]),
                        nombre = row["Nombre"].ToString(),
                        precioUnitario = Convert.ToDecimal(row["Precio"]) 
                    }
                });
            }

            return new JsonResult(new { success = false, message = "Producto no encontrado." });
        }
    }
}
