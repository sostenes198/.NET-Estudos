using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.Boundaries.UseCases.Outputs;
using Scheduled.Message.Application.Boundaries.UseCases.Validators;

namespace Scheduled.Message.Infrastructure.UseCases;

public class UseCaseManager(
    IServiceProvider serviceProvider,
    ILogger<UseCaseManager> logger) : IUseCaseManager
{
    public async Task ExecuteAsync<TInput, TOutput>(TInput input, TOutput output, CancellationToken token = default)
        where TInput : IUseCaseInput
        where TOutput : IUseCaseOutput
    {
        try
        {
            var useCase = serviceProvider.GetRequiredService<IUseCase<TInput, TOutput>>();

            if (output is IUseCaseOutputInvalidInput outputInvalidInput &&
                InvalidInput(input, outputInvalidInput, token))
            {
                return;
            }

            await useCase.ExecuteAsync(input, output, token);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Exception: {message}", exception.Message);

            if (output is IUseCaseOutputHandlerError outputUnhandledError)
            {
                outputUnhandledError.HandlerError(input, exception);
            }
            else
            {
                throw;
            }
        }
    }

    private bool InvalidInput<TInput, TOutput>(TInput input, TOutput output, CancellationToken token = default)
        where TInput : IUseCaseInput
        where TOutput : IUseCaseOutputInvalidInput
    {
        var useCaseInputValidator = serviceProvider.GetRequiredService<IUseCaseInputValidator<TInput>>();

        if (useCaseInputValidator.Validate(input, out var errors, token)) return false;

        output.InvalidInput(input, errors);
        return true;
    }
}