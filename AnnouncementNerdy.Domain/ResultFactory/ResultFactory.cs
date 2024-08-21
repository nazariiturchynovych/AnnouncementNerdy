using TestAssignment.Domain.Results;

namespace TestAssignment.Domain.ResultFactory;

public static class ResultFactory
{
    public static CommonResult Success()
        => new();

    public static CommonResult<TData> Success<TData>(TData data)
        => new(data);

    public static CommonResult Failure(string errorMessage, Exception? exception = null)
        => new(errorMessage, exception);

    public static CommonResult<TData> Failure<TData>(
        string errorMessage,
        Exception? exception = null)
        => new(errorMessage, exception);
}