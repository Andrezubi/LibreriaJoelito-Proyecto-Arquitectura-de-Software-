using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LibreriaJoelito.Aplicacion.Servicios;

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
            // Initial load
        }

        // Handler for AJAX search
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
                        cliente.Id, // Need ID to attach to the sale
                        cliente.Nombre,
                        cliente.ApellidoPaterno,
                        cliente.ApellidoMaterno
                    }
                });
            }

            return new JsonResult(new { success = false, message = "Cliente no encontrado" });
        }
    }
}
