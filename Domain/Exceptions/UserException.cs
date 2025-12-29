

namespace Domain.Exceptions;

public class UserException : Exception
{
    public int StatusCode { get; }

    public UserException(string message, int statusCode) : base (message)
    {
        StatusCode = statusCode;
    }

}
