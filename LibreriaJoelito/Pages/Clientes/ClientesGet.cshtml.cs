using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.FactoryCreators;
using LibreriaJoelito.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;
using LibreriaJoelito.Validators;

namespace LibreriaJoelito.Pages.Clientes
{
    public class ClientesGetModel : PageModel
    {
        private readonly IRepository<Cliente> _clienteRepository;

        public ClientesGetModel(IRepository<Cliente> clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public DataTable ClientesDataTable { get; set; } = new DataTable();

        [BindProperty]
        public Cliente _cliente { get; set; } = new();

        public void OnGet()
        {
            ClientesDataTable = _clienteRepository.GetAll();
        }

        // La creación ahora se maneja en Create.cshtml.cs

        public JsonResult OnPostUpdate()
        {
            try 
            {
                _cliente.Id = Convert.ToInt32(Request.Form["Id"]); // Asegurar ID del form hidden
                _cliente.Nombre = ClienteValidator.NormalizarTexto(_cliente.Nombre);
                _cliente.ApellidoPaterno = ClienteValidator.NormalizarTexto(_cliente.ApellidoPaterno);
                _cliente.ApellidoMaterno = ClienteValidator.NormalizarTexto(_cliente.ApellidoMaterno);

                var errores = ClienteValidator.Validar(_cliente);
                if (errores.Any())
                {
                    return new JsonResult(new { success = false, message = errores.First().ErrorMessage });
                }

                if (_clienteRepository is ClienteRepository repo && repo.ExisteDuplicado(_cliente))
                {
                    return new JsonResult(new { success = false, message = "Ya existe otro cliente con este CI y Complemento." });
                }

                _clienteRepository.Update(_cliente);
                TempData["MensajeExito"] = $"Cliente '{_cliente.Nombre} {_cliente.ApellidoPaterno}' actualizado exitosamente.";
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = "Error al actualizar: " + ex.Message });
            }
        }

        public IActionResult OnPostDelete(int id)
        {
            _cliente.Id = id;
            _clienteRepository.Delete(_cliente);
            TempData["MensajeExito"] = "Cliente eliminado correctamente.";
            return RedirectToPage();
        }
    }
}
