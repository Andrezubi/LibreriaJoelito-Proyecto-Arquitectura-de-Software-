using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.FactoryCreators;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Claims;

namespace LibreriaJoelito.Pages.Clientes
{
    [Authorize(Roles = "Administrador,Empleado")]
    public class ClientesGetModel : PageModel
    {
        private readonly ClienteServicio clienteServicio;
        private readonly ClienteValidator clienteValidator;

        public ClientesGetModel(ClienteServicio clienteServicio, ClienteValidator clienteValidator)
        {
            this.clienteServicio = clienteServicio;
            this.clienteValidator = clienteValidator;
        }

        public DataTable ClientesDataTable { get; set; } = new DataTable();

        [BindProperty]
        public Cliente _cliente { get; set; } = new();

        public void OnGet()
        {
            ClientesDataTable = clienteServicio.GetAll();
        }

        // La creación ahora se maneja en Create.cshtml.cs

        public JsonResult OnPostUpdate()
        {
            try 
            {
                _cliente.Id = Convert.ToInt32(Request.Form["Id"]); // Asegurar ID del form hidden
                _cliente.Nombre = clienteValidator.NormalizarTexto(_cliente.Nombre);
                _cliente.ApellidoPaterno = clienteValidator.NormalizarTexto(_cliente.ApellidoPaterno);
                _cliente.ApellidoMaterno = clienteValidator.NormalizarTexto(_cliente.ApellidoMaterno);
                _cliente.IdUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var result = clienteServicio.Update(_cliente);

                if (result.IsFailure)
                {
                    return new JsonResult(
                        new
                        {
                            success = false,
                            message = result.Errors.First()
                        }
                    );
                }
                
                TempData["MensajeExito"] =
                    $"Cliente '{_cliente.Nombre} {_cliente.ApellidoPaterno}' actualizado exitosamente.";

                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new
                {
                    success = false,
                    message = "Error al actualizar: " + ex.Message
                });
            }
        }

        public IActionResult OnPostDelete(int id)
        {
            _cliente.Id = id;
            _cliente.IdUsuario = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            clienteServicio.Delete(_cliente);
            TempData["MensajeExito"] = "Cliente eliminado correctamente.";
            return RedirectToPage();
        }
    }
}
