using tasktracker_3.DTO;
using tasktracker_3.Models;

namespace tasktracker_3.Help.Result
{
    public class Result<TModel,TDTO> where TModel : BaseModel
        where TDTO : BaseReadDTO
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
        public TDTO? ModelDTO { get; set;  }

        public List<TModel>? Models { get; set; }
        public List<TDTO>? ModelDTOs { get; set; }

        public static Result<TModel,TDTO> Success() => new(true, Error.None);

        public static Result<TModel,TDTO> Failure(Error error) => new(false, error);
    }
}
