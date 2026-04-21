using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
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
        public IActionResult OnGetExportarPdf(int idVenta)
        {
            if (idVenta <= 0)
            {
                return BadRequest("ID de venta inválido.");
            }

            var result = _ventaService.GenerarComprobantePdf(idVenta);

            if (result.IsFailure)
            {
                return Content($"Error: {string.Join(", ", result.Errors)}");
            }

            string nombreArchivo = $"Comprobante_Venta_{idVenta}.pdf";

            var contentDisposition = new System.Net.Mime.ContentDisposition
            {
                FileName = nombreArchivo,
                Inline = true
            };
            Response.Headers.Append("Content-Disposition", contentDisposition.ToString());

            return File(result.Value, "application/pdf");
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

        public JsonResult OnGetObtenerDetalleVenta(int idVenta)
        {
            try
            {
                var resultado = _ventaService.ObtenerVentaCompleta(idVenta);

                var venta = resultado.venta;
                var detalles = resultado.detalles;

                var listaDetalles = new List<object>();

                foreach (DataRow row in detalles.Rows)
                {
                    listaDetalles.Add(new
                    {
                        producto = row["NombreProducto"]?.ToString(),
                        presentacion = row["NombrePresentacion"]?.ToString(),
                        cantidad = Convert.ToInt32(row["Cantidad"]),
                        precioUnitario = Convert.ToDecimal(row["PrecioUnitario"]),
                        subtotal = Convert.ToDecimal(row["Subtotal"])
                    });
                }

                return new JsonResult(new
                {
                    success = true,
                    venta = new
                    {
                        idVenta = Convert.ToInt32(venta["Id"]),
                        ciCliente = venta["CiCliente"]?.ToString(),
                        nombreCliente = venta["NombreCliente"]?.ToString(),
                        fecha = Convert.ToDateTime(venta["Fecha"]).ToString("dd/MM/yyyy"),
                        empleado = venta["NombreEmpleado"]?.ToString(),
                        total = Convert.ToDecimal(venta["Total"])
                    },
                    detalles = listaDetalles
                });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }
    }
}