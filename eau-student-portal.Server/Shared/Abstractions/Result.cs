namespace eau_student_portal.Server.Shared.Abstractions;

public class Result : IResult
{
    public bool IsSuccess { get; private set; }
    public bool IsFailure => !IsSuccess;
    public string? ErrorMessage { get; private set; }

    protected Result(bool isSuccess, string? errorMessage = null)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result Success() => new(true);
    public static Result Failure(string errorMessage) => new(false, errorMessage);
}

public class Result<T> : Result, IResult<T>
{
    public T? Value { get; private set; }

    private Result(bool isSuccess, T? value, string? errorMessage = null)
        : base(isSuccess, errorMessage)
    {
        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value);
    public static new Result<T> Failure(string errorMessage) => new(false, default, errorMessage);
}

