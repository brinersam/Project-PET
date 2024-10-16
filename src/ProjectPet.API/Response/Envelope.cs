namespace ProjectPet.API.Response
{
    public record Envelope
    {
        public object? Result { get; }
        public DateTime? TimeGenerated { get; }
        public object? Errors { get; } = null;
        protected Envelope(object? result)
        {
            Result = result;
            TimeGenerated = DateTime.Now;
        }

        public static Envelope Ok(object? result = null)
            => new Envelope(result);

        public static Envelope<T> Error<T>(IEnumerable<T> errors)
            => Envelope<T>.Create(errors);
    }

    public record Envelope<T> : Envelope
    {
        new public List<T> Errors { get; }

        private Envelope(object? result,IEnumerable<T> errors) : base(result)
        {
            Errors = errors.ToList();
        }

        public static Envelope<T> Create(IEnumerable<T> errors)
            => new Envelope<T>(null, errors);
    }

    public record ResponseError(
        string? ErrorCode,
        string? ErrorMessage,
        string? InvalidField);
}
