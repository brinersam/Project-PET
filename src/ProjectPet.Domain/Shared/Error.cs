namespace ProjectPet.Domain.Shared
{
    public class Error
    {
        private const string DELIMITER = "|";
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

        public static bool TryDeserialize(string err, out Error result)
        {
            string[] split = err.Split(DELIMITER);
            if (split.Length != 3)
            {
                result = null!;
                return false;
            }

            ErrorType errType = Enum.Parse<ErrorType>(split[2]);
            result = new Error(split[0], split[1], errType);
            return true;
        }

        public static Error Validation(string code, string message) =>
             new Error(code, message, ErrorType.Validation);
        public static Error NotFound(string code, string message) =>
             new Error(code, message, ErrorType.NotFound);
        public static Error Failure(string code, string message) =>
             new Error(code, message, ErrorType.Failure);
        public static Error Conflict(string code, string message) =>
             new Error(code, message, ErrorType.Conflict);
    }

    public static class ErrorExtensions
    {
        public static string Serialize(this Error err)
        {
            return $"{err.Code}|{err.Message}|{err.Type.ToString()}";
        }
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
