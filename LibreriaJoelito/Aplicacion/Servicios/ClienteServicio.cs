using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Results;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Servicios
{
    public class ClienteServicio
    {
        private readonly IRepository<Cliente> clienteRepository;
        private readonly ClienteValidator clienteValidator;

        public ClienteServicio(IRepository<Cliente> clienteRepository, ClienteValidator clienteValidator)
        {
            this.clienteRepository = clienteRepository;
            this.clienteValidator = clienteValidator;
        }

        public DataTable GetAll()
        {
            return clienteRepository.GetAll();
        }

        public DataRow GetById(int id)
        {
            return clienteRepository.GetById(id);
        }

        public Result Insert(Cliente cliente)
        {
            var validationResults = clienteValidator.Validar(cliente);

            if (validationResults.Any())
            {
                var errors = validationResults
                    .Select(v =>
                    {
                        var field = v.MemberNames.FirstOrDefault() ?? "General";
                        return $"{field}: {v.ErrorMessage}";
                    })
                    .ToList();

                return Result.Failure(errors);
            }

            if (clienteRepository.ExisteDuplicado(cliente))
            {
                return Result.Failure("_cliente.Ci: Ya existe un cliente con este CI y Complemento.");
            }
            // Temporal: asignar un usuario por defecto hasta que se implemente autenticación
            cliente.IdUsuario = 1; // reemplazar por id del usuario autenticado

            clienteRepository.Insert(cliente);

            return Result.Success();
        }

        public Result Update(Cliente cliente)
        {
            var validationResults = clienteValidator.Validar(cliente);

            if (validationResults.Any())
            {
                var errors = validationResults
                    .Select(v =>
                    {
                        return $"{v.ErrorMessage}";
                    })
                    .ToList();

                return Result.Failure(errors);
            }

            if (clienteRepository.ExisteDuplicado(cliente))
            {
                return Result.Failure("Ya existe un cliente con este CI y Complemento.");
            }

            clienteRepository.Update(cliente);

            return Result.Success();
        }

        public int Delete(Cliente cliente)
        {
            return clienteRepository.Delete(cliente);
        }
    }
}
