using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Results;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using LibreriaJoelito.Infraestructura.Persistencia.FactoryProducts;
using MySqlX.XDevAPI;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Servicios
{
    public class UsuarioServicio
    {
        private readonly IRepository<Empleado> usuarioRepository;
        private readonly EmpleadoValidator usuarioValidator;

        public UsuarioServicio(IRepository<Empleado> usuarioRepository, EmpleadoValidator usuarioValidator)
        {
            this.usuarioRepository = usuarioRepository;
            this.usuarioValidator = usuarioValidator;
        }

        public DataTable GetAll()
        {
            return usuarioRepository.GetAll();
        }

        public DataRow GetById(int id)
        {
            return usuarioRepository.GetById(id);
        }

        public Result Insert(Empleado usuario)
        {
            var validationResults = usuarioValidator.Validar(usuario);

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

            if (usuarioRepository.ExisteDuplicado(usuario))
            {
                return Result.Failure("empleado.Ci: El empleado con ese CI ya existe.");
            }

            usuarioRepository.Insert(usuario);

            return Result.Success();
        }

        public Result Update(Empleado usuario)
        {
            var validationResults = usuarioValidator.Validar(usuario);

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

            if (usuarioRepository.ExisteDuplicado(usuario))
            {
                return Result.Failure("empleado.Ci: El empleado con ese CI ya existe.");
            }

            usuarioRepository.Update(usuario);

            return Result.Success();
        }

        public int Delete(Empleado usuario)
        {
            return usuarioRepository.Delete(usuario);
        }
    }
}
