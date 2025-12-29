

using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class UserCreationException : UserException
{
    public UserCreationException(string errors): base($"Errores: {errors}", StatusCodes.Status400BadRequest) { }
}
