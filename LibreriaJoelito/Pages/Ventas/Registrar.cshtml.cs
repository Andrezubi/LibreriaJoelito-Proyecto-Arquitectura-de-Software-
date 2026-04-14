using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
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

        // --- HU-03 Role B: Quick Client Creation ---
        public JsonResult OnPostCrearClienteRapido([FromBody] Cliente cliente)
        {
            if (cliente == null)
                return new JsonResult(new { success = false, message = "Datos de cliente no recibidos" });

            // Asignar auditoria básica (Usuario actual)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null) {
                cliente.IdUsuario = int.Parse(userIdClaim.Value);
            } else {
                cliente.IdUsuario = 1; // Fallback para desarrollo si no hay auth activa
            }

            cliente.ClienteFrecuente = false;

            var result = _clienteServicio.Insert(cliente);

            if (result.IsSuccess)
            {
                return new JsonResult(new { 
                    success = true, 
                    id = result.Value, 
                    nombre = $"{cliente.Nombre} {cliente.ApellidoPaterno}"
                });
            }

            return new JsonResult(new { success = false, errors = result.Errors });
        }
    }
}
