namespace LibreriaJoelito.Aplicacion.Results
{
    public class Result
    {
        //public bool esValido { get; set; }
        //public string mensaje { get; set; }

        //protected Result(bool esValido, string mensaje)
        //{
        //    this.esValido = esValido;
        //    this.mensaje = mensaje;
        //}

        //public static Result Valido()
        //{
        //    return new Result(true, "Validación exitosa.");
        //}

        //public static Result Invalido(string mensaje)
        //{
        //    return new Result(false, mensaje);
        //}

        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public List<string> Errors { get; }

        protected Result(bool isSuccess, List<string> errors)
        {
            //if (isSuccess && errors.Any())
            //    throw new InvalidOperationException("Un resultado exitoso no puede tener errores");

            //if (!isSuccess && !errors.Any())
            //    throw new InvalidOperationException("Un resultado fallido debe tener al menos un error");

            IsSuccess = isSuccess;
            Errors = errors;
        }

        public static Result Success()
        {
            return new Result(true, new List<string>()); ;
        }

        public static Result Failure(List<string> errors)
        {
            return new Result(false, errors);
        }

        public static Result Failure(string error)
        {
            return new Result(false, new List<string> { error });
        }
    }
}
