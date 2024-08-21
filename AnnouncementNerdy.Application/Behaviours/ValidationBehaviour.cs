using AnnouncementNerdy.Domain.Errors;
using AnnouncementNerdy.Domain.Results;
using FluentValidation;

namespace AnnouncementNerdy.Application.Behaviours;

public class ValidationPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : CommonResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)

    {
        if (!_validators.Any())
            return await next();

        string[] errors = _validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => failure.ErrorMessage)
            .Distinct()
            .ToArray();

        if (errors.Any())
        {
            return CreateValidationResult<TResponse>(errors);
        }

        return await next();
    }


    private static TResult CreateValidationResult<TResult>(string[] errors)
        where TResult : CommonResult
    {
        if (typeof(TResult) == typeof(CommonResult))
            return (new ValidationResult(CommonErrors.ValidationProblemAreNotOccured, errors) as TResult)!;

        object validationResult = typeof(CommonValidationResultTData<>)
            .GetGenericTypeDefinition()
            .MakeGenericType(typeof(TResult).GenericTypeArguments[0])
            .GetConstructor(new[] {typeof(string), typeof(string[]), typeof(Exception)})!
            .Invoke(new object[] {CommonErrors.ValidationProblemAreNotOccured, errors, null!});

        return (TResult) validationResult;
    }
}