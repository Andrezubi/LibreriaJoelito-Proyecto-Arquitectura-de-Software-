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
        private readonly IClienteRepository clienteRepository;
        private readonly ClienteValidator clienteValidator;

        public ClienteServicio(IClienteRepository clienteRepository, ClienteValidator clienteValidator)
        {
            this.clienteRepository = clienteRepository;
            this.clienteValidator = clienteValidator;
        }

        public Cliente? BuscarPorCi(string ci)
        {
            var row = clienteRepository.GetByCi(ci);
            if (row == null) return null;

            return new Cliente
            {
                Id = Convert.ToInt32(row["Id"]),
                Nombre = row["Nombre"].ToString(),
                ApellidoPaterno = row["ApellidoPaterno"].ToString(),
                ApellidoMaterno = row["ApellidoMaterno"]?.ToString(),
                Ci = row["Ci"].ToString(),
                Complemento = row["Complemento"]?.ToString(),
                Email = row["Email"]?.ToString(),
                ClienteFrecuente = Convert.ToBoolean(row["ClienteFrecuente"])
            };
        }

        public DataTable GetAll()
        {
            return clienteRepository.GetAll();
        }

        public DataRow GetById(int id)
        {
            return clienteRepository.GetById(id);
        }

        public Result<int> Insert(Cliente cliente)
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

                return Result<int>.Failure(errors);
            }

            if (clienteRepository.ExisteDuplicado(cliente))
            {
                return Result<int>.Failure("_cliente.Ci: Ya existe un cliente con este CI y Complemento.");
            }
        

            int idGenerado = clienteRepository.Insert(cliente);

            return Result<int>.Success(idGenerado);
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
