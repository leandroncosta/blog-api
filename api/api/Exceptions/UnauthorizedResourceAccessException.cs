namespace api.Exceptions
{
    public class UnauthorizedResourceAccessException : Exception
    {
        public UnauthorizedResourceAccessException(String message) : base(message) { }
        public UnauthorizedResourceAccessException(string message, Exception innerException) : base(message, innerException) { }
    }
}
