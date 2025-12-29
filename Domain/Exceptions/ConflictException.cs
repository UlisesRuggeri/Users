

using Microsoft.AspNetCore.Http;

namespace Domain.Exceptions;

public class ConflictException : UserException
{
    public ConflictException() : base("Error al eliminar el recurso", StatusCodes.Status409Conflict) { }
}
