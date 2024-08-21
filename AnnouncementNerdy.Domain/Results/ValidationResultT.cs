namespace TestAssignment.Domain.Results;

public record CommonValidationResultTData<TData> : CommonResult<TData>
{

    public CommonValidationResultTData(string errorMessage, string[] errors, Exception? exception = null)
        : base(errorMessage, exception)
    {
        Errors = errors;
    }

    public string[] Errors { get; set; }
}