using TestAssignment.Domain.Results.Abstract;

namespace TestAssignment.Domain.Results;

public record CommonResult<TData> : CommonResult, ICommonResult<TData>
{

    public CommonResult(TData data)
    {
        Data = data;
    }

    public CommonResult(
        string errorMessage,
        Exception? exception = null)
        : base(errorMessage, exception)
    {
        Data = default!;
    }

    public TData Data { get; }
}