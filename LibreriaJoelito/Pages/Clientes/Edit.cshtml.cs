using LibreriaJoelito.FactoryCreators;
using LibreriaJoelito.FactoryProducts;
using LibreriaJoelito.Models;
using LibreriaJoelito.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using System.Data;

namespace LibreriaJoelito.Pages.Clientes
{
    public class EditModel : PageModel
    {
        private readonly IRepository<Cliente> _clienteRepository = new ClienteRepositoryCreator().CreateRepository();
        
        [BindProperty]
        public Cliente _cliente { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            DataRow? fila = _clienteRepository.GetByID(id);
            if (fila is null)
            {
                return NotFound();
            }

            _cliente.Id = Convert.ToInt32(fila["Id"]);
            _cliente.Nombre = fila["Nombre"].ToString() ?? "";
            _cliente.ApellidoPaterno = fila["ApellidoPaterno"].ToString() ?? "";
            _cliente.ApellidoMaterno = fila["ApellidoMaterno"] == DBNull.Value ? null : fila["ApellidoMaterno"].ToString();
            _cliente.CI = fila["Ci"].ToString() ?? "";
            _cliente.Complemento = fila["Complemento"] == DBNull.Value ? null : fila["Complemento"].ToString();
            _cliente.Email = fila["Email"] == DBNull.Value ? null : fila["Email"].ToString();
            _cliente.EsClienteFrecuente = Convert.ToBoolean(fila["ClienteFrecuente"]);

            return Page();
        }

        public IActionResult OnPost()
        {
            // Normalización
            _cliente.Nombre = ClienteValidator.NormalizarTexto(_cliente.Nombre);
            _cliente.ApellidoPaterno = ClienteValidator.NormalizarTexto(_cliente.ApellidoPaterno);
            _cliente.ApellidoMaterno = ClienteValidator.NormalizarTexto(_cliente.ApellidoMaterno);

            // Validación
            var errores = ClienteValidator.Validar(_cliente);
            if (errores.Any())
            {
                foreach (var err in errores)
                {
                    ModelState.AddModelError(err.MemberNames.First(), err.ErrorMessage ?? "Error");
                }
                return Page();
            }

            // Verificar Duplicados (excluyendo el actual)
            if (_clienteRepository is ClienteRepository repo && repo.ExisteDuplicado(_cliente))
            {
                ModelState.AddModelError("Cliente.CI", "Ya existe otro cliente registrado con este CI y Complemento.");
                return Page();
            }

            _clienteRepository.Update(_cliente);
            TempData["MensajeExito"] = $"Cliente '{_cliente.Nombre} {_cliente.ApellidoPaterno}' modificado.";

            return RedirectToPage("ClientesGet");
        }
    }
}
