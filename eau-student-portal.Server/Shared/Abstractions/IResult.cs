namespace eau_student_portal.Server.Shared.Abstractions;

public interface IResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    string? ErrorMessage { get; }
}

public interface IResult<T> : IResult
{
    T? Value { get; }
}

