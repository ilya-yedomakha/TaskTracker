using tasktracker_3.Models;

namespace tasktracker_3.Help.Result.ModelErrors
{
    public class ModelError<T> where T : BaseModel
    {
        public static Error SameTitle(String title) => new Error(
            $"{typeof(T).Name}.SameTitle", $"{typeof(T).Name} with title: " + title + " already exists");

        public static Error NotFound(long id) => new Error(
            $"{typeof(T).Name}.NotFound", $"{typeof(T).Name} with the id: " + id + " was not found");

        public static readonly Error NullReference = new Error(
            $"{typeof(T).Name}.NullReference", $"{typeof(T).Name} reference is required");

        public static readonly Error ServerError = new Error(
            $"{typeof(T).Name}.ServerError", "Something went wrong!");
    }
}
