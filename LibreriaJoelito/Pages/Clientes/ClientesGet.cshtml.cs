using LibreriaJoelito.Infraestructura.FactoryCreators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;
using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Servicios;

namespace LibreriaJoelito.Pages.Clientes
{
    public class ClientesGetModel : PageModel
    {
        //private readonly IRepository<Cliente> _clienteRepository;
        private readonly ClienteServicio clienteServicio;
        private readonly ClienteValidator clienteValidator;

        //public ClientesGetModel(IRepository<Cliente> clienteRepository)
        //{
        //    _clienteRepository = clienteRepository;
        //}

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

                //var errores = ClienteValidator.Validar(_cliente);
                //if (errores.Any())
                //{
                //    return new JsonResult(new { success = false, message = errores.First().ErrorMessage });
                //}

                //if (_clienteRepository is ClienteRepository repo && repo.ExisteDuplicado(_cliente))
                //{
                //    return new JsonResult(new { success = false, message = "Ya existe otro cliente con este CI y Complemento." });
                //}

                //_clienteRepository.Update(_cliente);
                //TempData["MensajeExito"] = $"Cliente '{_cliente.Nombre} {_cliente.ApellidoPaterno}' actualizado exitosamente.";
                //return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                //return new JsonResult(new { success = false, message = "Error al actualizar: " + ex.Message });
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
            clienteServicio.Delete(_cliente);
            TempData["MensajeExito"] = "Cliente eliminado correctamente.";
            return RedirectToPage();
        }
    }
}
