namespace PersonalBlog.API.Exceptions
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message)
            : base(message, 404, "NOT_FOUND")
        {
        }

        public NotFoundException(string entityName, int id)
            : base($"{entityName} with ID {id} was not found.", 404, "NOT_FOUND")
        {
        }
    }
}
