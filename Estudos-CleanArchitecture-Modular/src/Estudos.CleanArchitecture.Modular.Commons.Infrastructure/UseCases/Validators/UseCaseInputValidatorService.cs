using FluentValidation;
using Microsoft.Extensions.Logging;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.Validators;

namespace Estudos.CleanArchitecture.Modular.Infrastructure.UseCases.Validators;

internal sealed class UseCaseInputValidatorService<TInput> : IUseCaseInputValidatorService<TInput>
    where TInput : UseCaseInput
{
    private readonly IValidator<TInput> _validator;
    private readonly ILogger<UseCaseInputValidatorService<TInput>> _logger;

    public UseCaseInputValidatorService(IValidator<TInput> validator, ILogger<UseCaseInputValidatorService<TInput>> logger) => (_validator, _logger) = (validator, logger);

    public bool Validate(TInput input, out NotificationsInputError notificationsInputError, CancellationToken cancellationToken = default)
    {
        notificationsInputError = NotificationsInputError.Empty;
        var result = _validator.Validate(input);

        if (result.IsValid)
            return result.IsValid;

        foreach (var error in result.Errors)
        {
            notificationsInputError.Add(error.PropertyName, error.ErrorMessage);
        }

        _logger.LogInformation("Input {Input} inválido", nameof(input));

        return result.IsValid;
    }
}