namespace AnnouncementNerdy.Domain.Results;

public record ValidationResult : CommonResult
{

    public ValidationResult(string errorMessage, string[] errors, Exception? exception = null)
        : base(errorMessage, exception)
    {
        Errors = errors;
    }

    public string[] Errors { get; }
}