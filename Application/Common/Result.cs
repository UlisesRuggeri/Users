namespace Application.Common;

public class Result<T>
{
    public T? Value { get; }
    public bool IsSucces { get;}
    public string? Error { get; }

    private Result(T? value, bool isSucces, string? error)
    {
        Value = value;
        IsSucces = isSucces;
        Error = error;
    }

    public static Result<T> Succes(T? value) => new(value, true, null);
    public static Result<T> Failure(string error) => new(default, false, error);
}
