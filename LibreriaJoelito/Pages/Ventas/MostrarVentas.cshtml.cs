using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Infraestructura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Pages.Ventas
{
    [Authorize(Roles = "Administrador,Empleado")]
    public class MostrarVentasModel : PageModel
    {
        public DataTable VentasDataTable { get; set; } = new DataTable();

        private readonly VentaService _ventaService;
        private readonly IPdfService _pdfService;
        public MostrarVentasModel(VentaService ventaService, IPdfService pdfService)
        {
            _ventaService = ventaService;
            _pdfService = pdfService;
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
            if (id <= 0)
                return RedirectToPage();

            // 1. Obtener detalle de la venta (debes tener este método)
            DataTable dt = _ventaService.ObtenerDetalleVenta(id);

            if (dt.Rows.Count == 0)
                return RedirectToPage();

            // 2. Generar PDF
            byte[] pdfBytes = _pdfService.GenerarComprobanteVenta(dt);

            // 3. Retornar archivo
            return File(pdfBytes, "application/pdf", $"Comprobante_Venta_{id}.pdf");
        }

        public IActionResult OnPostAnular(int idVenta)
        {
            if (idVenta <= 0) return RedirectToPage();

            var resultado = _ventaService.AnularVenta(idVenta);

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