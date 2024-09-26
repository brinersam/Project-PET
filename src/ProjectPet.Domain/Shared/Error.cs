namespace ProjectPet.Domain.Shared
{
    public class Error
    {
        public string Code { get; }
        public string Message { get; }
        public ErrorType Type { get; }

        private Error(string code, string message, ErrorType type)
        {
            if (type == ErrorType.Not_set)
                throw new ArgumentException("Error was created with no ErrorType set!");

            Code = code;
            Message = message;
            Type = type;
        }

        public static Error Validation(string code, string message) =>
             new Error(code,message,ErrorType.Validation);
        public static Error NotFound(string code, string message) =>
             new Error(code, message, ErrorType.NotFound);
        public static Error Failure(string code, string message) =>
             new Error(code, message, ErrorType.Failure);
        public static Error Conflict(string code, string message) =>
             new Error(code, message, ErrorType.Conflict);
    }

    public enum ErrorType
    {
        Not_set,
        Validation = 422,
        NotFound = 404,
        Failure = 500,
        Conflict
    }
}
