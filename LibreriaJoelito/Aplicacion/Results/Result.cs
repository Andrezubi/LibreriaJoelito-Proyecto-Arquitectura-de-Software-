namespace LibreriaJoelito.Aplicacion.Results
{
    public class Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public List<string> Errors { get; }

        protected Result(bool isSuccess, List<string> errors)
        {
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
