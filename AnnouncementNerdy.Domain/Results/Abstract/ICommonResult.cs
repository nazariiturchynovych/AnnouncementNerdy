namespace TestAssignment.Domain.Results.Abstract;

public interface ICommonResult
{
    bool IsSuccess { get; }

    bool IsFailure { get; }

    string? ExceptionMessage { get; }

    string ErrorMessage { get; }
}
