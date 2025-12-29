

using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class UserNotFoundException : UserException
{
    public UserNotFoundException(string email) : base($"Usuario con el ID {email} no encontrado", StatusCodes.Status404NotFound) { }
}
