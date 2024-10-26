using tasktracker_3.Models;

namespace tasktracker_3.Help.Result
{
    public class Result<TModel> where TModel : BaseModel
    {
        private Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error Error { get; }

        public TModel? Model { get; set;  }

        public List<TModel>? Models { get; set; }

        public static Result<TModel> Success() => new(true, Error.None);

        public static Result<TModel> Failure(Error error) => new(false, error);
    }
}
