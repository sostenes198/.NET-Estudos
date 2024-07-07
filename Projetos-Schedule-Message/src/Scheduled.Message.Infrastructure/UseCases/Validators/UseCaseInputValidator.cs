using FluentValidation;
using Microsoft.Extensions.Logging;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.Boundaries.UseCases.Validators;

namespace Scheduled.Message.Infrastructure.UseCases.Validators;

public class UseCaseInputValidator<TInput>(
    IValidator<TInput> validator,
    ILogger<UseCaseInputValidator<TInput>> logger)
    : IUseCaseInputValidator<TInput>
    where TInput : IUseCaseInput
{
    public bool Validate(TInput input, out NotificationsInputError notificationErrors,
        CancellationToken token = default)
    {
        notificationErrors = NotificationsInputError.Empty;
        var result = validator.Validate(input);

        if (result.IsValid)
            return result.IsValid;

        foreach (var error in result.Errors)
        {
            notificationErrors.Add(error.PropertyName, error.ErrorMessage);
        }

        logger.LogInformation("Invalid input: {Input}", nameof(input));

        return result.IsValid;
    }
}