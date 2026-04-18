using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Security.Claims;

namespace LibreriaJoelito.Pages.Ventas
{
    public class RegistrarModel : PageModel
    {
        private readonly ClienteServicio _clienteServicio;

        public RegistrarModel(ClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
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
                return new JsonResult(new { success = false, message = "Datos inválidos" });
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
    }
}
