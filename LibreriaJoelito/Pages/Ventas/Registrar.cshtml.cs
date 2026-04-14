using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LibreriaJoelito.Pages.Ventas
{
    public class RegistrarModel : PageModel
    {
        private readonly ClienteServicio _clienteServicio;

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
    }
}
