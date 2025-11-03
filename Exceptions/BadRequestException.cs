namespace PersonalBlog.API.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message)
            : base(message, 400, "BAD_REQUEST")
        {
        }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException, 400, "BAD_REQUEST")
        {
        }
    }
}
