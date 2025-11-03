namespace PersonalBlog.API.Exceptions
{
    public class ValidationException : BaseException
    {
        public Dictionary<string, string[]> Errors { get; }

        public ValidationException(Dictionary<string, string[]> errors)
            : base("Validation failed. One or more errors occurred.", 400, "VALIDATION_ERROR")
        {
            Errors = errors;
        }

        public ValidationException(string field, string errorMessage)
            : base("Validation failed. One or more errors occurred.", 400, "VALIDATION_ERROR")
        {
            Errors = new Dictionary<string, string[]>
            {
                { field, new[] { errorMessage } }
            };
        }
    }
}
