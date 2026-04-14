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
            DataRow row = clienteRepository.GetByCi(ci);
            if (row != null)
            {
                return new Cliente
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Nombre = row["Nombre"].ToString(),
                    ApellidoPaterno = row["ApellidoPaterno"].ToString(),
                    // add other properties if needed, but for HU-03 we only need Nombre and Apellido
                    // according to the table, columns returned by GetByCi are: 
                    // Id, Nombre, ApellidoPaterno, ApellidoMaterno, Ci, Complemento, Email, ClienteFrecuente, FechaRegistro
                    ApellidoMaterno = row["ApellidoMaterno"] == DBNull.Value ? null : row["ApellidoMaterno"].ToString(),
                    Ci = row["Ci"].ToString(),
                    Complemento = row["Complemento"] == DBNull.Value ? null : row["Complemento"].ToString()
                };
            }
            return null;
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
