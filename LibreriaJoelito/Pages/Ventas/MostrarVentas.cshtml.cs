using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Infraestructura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Claims;

namespace LibreriaJoelito.Pages.Ventas
{
    [Authorize(Roles = "Administrador,Empleado")]
    public class MostrarVentasModel : PageModel
    {
        public DataTable VentasDataTable { get; set; } = new DataTable();

        private readonly VentaService _ventaService;

        public MostrarVentasModel(VentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [TempData]
        public string MensajeExito { get; set; }

        public void OnGet()
        {
            VentasDataTable = _ventaService.LoadVentas();
        }

        

        // EXPORTAR PDF
        public IActionResult OnGetExportarPdf(int id)
        {
            // aquí luego puedes generar el PDF
            return RedirectToPage(); // placeholder
        }

        public IActionResult OnPostAnular(int idVenta)
        {
            if (idVenta <= 0) return RedirectToPage();

            int idEmpleado= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var resultado = _ventaService.AnularVenta(idVenta, idEmpleado);

            if (resultado.IsSuccess)
            {
                MensajeExito = $"La venta #{idVenta} ha sido anulada y el stock fue restaurado correctamente.";
            }
            else
            {
                MensajeExito = $"Hubo un problema al anular: {string.Join(", ", resultado.Errors)}";
            }

            return RedirectToPage();
        }
    }
}