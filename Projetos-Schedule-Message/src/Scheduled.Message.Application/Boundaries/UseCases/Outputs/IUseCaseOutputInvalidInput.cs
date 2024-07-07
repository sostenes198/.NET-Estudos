using Scheduled.Message.Application.Boundaries.UseCases.Validators;

namespace Scheduled.Message.Application.Boundaries.UseCases.Outputs;

public interface IUseCaseOutputInvalidInput : IUseCaseOutput
{
    void InvalidInput<TUseCaseInput>(TUseCaseInput input, NotificationsInputError errors)
        where TUseCaseInput : IUseCaseInput;
}