namespace Application.Common;

public class Result<T>
{
    public T? Value { get; }
    public bool IsSuccess { get; }
    public string? Message { get; }
    public string? Error { get; }

    private Result(T? value, bool isSuccess, string? message, string? error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Message = message;
        Error = error;
    }

    public static Result<T> Succes(T? value, string? message = null) => new(value, true, message, null);

    public static Result<T> Failure(string? error = null, T? value = default) => new(value, false, null, error);
}
