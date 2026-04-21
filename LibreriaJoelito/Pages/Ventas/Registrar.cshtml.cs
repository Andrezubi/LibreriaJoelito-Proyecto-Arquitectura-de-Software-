using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Security.Claims;

namespace LibreriaJoelito.Pages.Ventas
{
    public class RegistrarModel : PageModel
    {
        private readonly ClienteServicio _clienteServicio;
        private readonly ProductoServicio _productoServicio;
        private readonly IVentaService _FachadaVentas;

        public RegistrarModel(ClienteServicio clienteServicio, ProductoServicio productoServicio, IVentaService fachadaVentas)
        {
            _clienteServicio = clienteServicio;
            _productoServicio = productoServicio;
            _FachadaVentas = fachadaVentas;
        }

        public void OnGet()
        {
        }

        // --- HU-03 Role A: Real-time search by CI ---
        public JsonResult OnGetBuscarCliente(string ci)
        {
            if (string.IsNullOrWhiteSpace(ci))
                return new JsonResult(new { success = false, message = "CI no proporcionado" });
            
            DataTable clientesSimilares = _clienteServicio.GetAllSimilarId(ci);
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
                return new JsonResult(new { success = false, message = "Datos inv�lidos" });
            }

            cliente.Estado = true;
            cliente.FechaRegistro = DateTime.Now;

            cliente.IdUsuario = 1;

            var result = _clienteServicio.Insert(cliente);
            if (result.IsFailure)
            {
                string fullErrorMessage="";
                foreach (var error in result.Errors)
                {
                    var parts = error.Split(':', 2);

                    if (parts.Length == 2)
                    {
                        var field = parts[0].Trim();
                        var message = parts[1].Trim();
                        fullErrorMessage += $"Error in {field}: {message} \n";
                        
                    }
                    else
                    {
                        fullErrorMessage+= $"Error: {error} \n";
                    }
                }
                return new JsonResult(new
                {
                    success = false,
                    message = fullErrorMessage
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
        public JsonResult OnGetBuscarClientesParcial(string ci)
        {
            if (string.IsNullOrWhiteSpace(ci))
            {
                return new JsonResult(new { success = false, clientes = new List<object>() });
            }

            var tabla = _clienteServicio.GetAllSimilarId(ci);

            var lista = new List<object>();

            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(new
                {
                    id = Convert.ToInt32(row["Id"]),
                    nombre = row["Nombre"].ToString(),
                    apellidoPaterno = row["ApellidoPaterno"].ToString(),
                    apellidoMaterno = row["ApellidoMaterno"] == DBNull.Value ? null : row["ApellidoMaterno"].ToString(),
                    ci = row["Ci"].ToString()
                });
            }

            return new JsonResult(new
            {
                success = true,
                clientes = lista
            });
        }
        public JsonResult OnGetBuscarNombre(string termino)
        {
            DataTable dt = _FachadaVentas.getPresentacionProductosByFrase(termino);
            var listaNombres = new List<object>();
            foreach (DataRow row in dt.Rows)
            {

                listaNombres.Add(new
                {
                    texto = row["Presentacion"] + " de " + row["Producto"] + " " + row["Marca"],
                    idProducto = row["IdProducto"],
                    idPresentacion = row["IdPresentacion"]
                });
            }

            return new JsonResult(listaNombres);
        }
        public IActionResult OnGetObtenerDetalleProducto(string frase,int idProducto, int idPresentacion)
        {
            if (string.IsNullOrEmpty(frase))
            {
                return new JsonResult(new { success = false, message = "El nombre esta vacio." });
            }
            return _FachadaVentas.getPresentacionProductoByIds(idProducto,idPresentacion);
            
        }

        public class RegistrarVentaDto
        {
            public int IdCliente { get; set; }
            public List<DetalleVenta> Detalles { get; set; }
        }

        [ValidateAntiForgeryToken]
        public JsonResult OnPostRegistrarVenta([FromBody] RegistrarVentaDto dto)
        {
            if (dto == null || dto.Detalles == null || !dto.Detalles.Any())
                return new JsonResult(new { success = false, message = "La venta no tiene productos." });

            if (dto.IdCliente <= 0)
                return new JsonResult(new { success = false, message = "Cliente no válido." });

            var venta = new Venta(
                idCliente: dto.IdCliente,
                idUsuario: 1,
                fecha: DateTime.Now,
                total: dto.Detalles.Sum(d => d.Cantidad * d.PrecioUnitario),
                estado: true
            );
            var result = _FachadaVentas.RegistrarVenta(venta, dto.Detalles);

            if (result.IsSuccess)
                return new JsonResult(new { success = true, idVenta = result.Value, message = "Venta registrada correctamente." });

            return new JsonResult(new { success = false, message = result.Errors.FirstOrDefault() ?? "Error al registrar." });
        }
    }
}
