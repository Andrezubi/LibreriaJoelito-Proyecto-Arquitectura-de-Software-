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

        public RepositorioBD bd { get; set; } = RepositorioBD.Instancia;

        private readonly VentaService _ventaService;

        public MostrarVentasModel(VentaService ventaService)
        {
            _ventaService = ventaService;
        }

        [TempData]
        public string MensajeExito { get; set; }

        public void OnGet()
        {
            LoadVentas();
        }

        void LoadVentas()
        {
            string query = @"
        SELECT v.Id,
               c.Ci AS CiCliente,
               v.Fecha,
               v.Total,
               u.Nombre AS NombreEmpleado
        FROM venta v
        INNER JOIN cliente c ON v.IdCliente = c.Id
        INNER JOIN usuario u ON v.IdUsuario = u.Id
        ORDER BY v.Fecha DESC";

            MySqlCommand cmd = new MySqlCommand(query);

            VentasDataTable = bd.ExecuteReturningDataTable(cmd);
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