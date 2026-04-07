using LibreriaJoelito.Aplicacion.Interfaces;
using LibreriaJoelito.Aplicacion.Results;
using LibreriaJoelito.Dominio.Models;
using LibreriaJoelito.Dominio.Validators;
using System.Data;

namespace LibreriaJoelito.Aplicacion.Servicios
{
    public class MarcaServicio
    {
        private readonly IRepository<Marca> marcaRepository;
        private readonly MarcaValidator marcaValidator;

        public MarcaServicio(IRepository<Marca> marcaRepository, MarcaValidator marcaValidator)
        {
            this.marcaRepository = marcaRepository;
            this.marcaValidator = marcaValidator;
        }

        public DataTable GetAll()
        {
            return marcaRepository.GetAll();
        }

        public Result Insert(Marca marca)
        {
            var validationResults = marcaValidator.Validar(marca);

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

            if (marcaRepository.ExisteDuplicado(marca))
            {
                return Result.Failure("Marca.Nombre: Ya existe una marca registrada con este nombre.");
            }

            marca.IdUsuario = 1; //CAMBIAR POR EL USUARIO LOGUEADO UNA VES MERGEADO CON AUTENTICACION

            marcaRepository.Insert(marca);

            return Result.Success();
        }

        public Result Update(Marca marca)
        {
            var validationResults = marcaValidator.Validar(marca);

            if (validationResults.Any())
            {
                var errors = validationResults
                    .Select(v => $"{v.ErrorMessage}")
                    .ToList();

                return Result.Failure(errors);
            }

            if (marcaRepository.ExisteDuplicado(marca))
            {
                return Result.Failure("Ya existe una marca registrada con este nombre.");
            }

            marcaRepository.Update(marca);

            return Result.Success();
        }

        public int Delete(Marca marca)
        {
            return marcaRepository.Delete(marca);
        }
    }
}