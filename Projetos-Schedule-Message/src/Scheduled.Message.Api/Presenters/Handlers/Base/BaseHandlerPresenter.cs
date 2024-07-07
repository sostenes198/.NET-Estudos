using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.Boundaries.UseCases.Outputs;
using Scheduled.Message.Application.Boundaries.UseCases.Validators;

namespace Scheduled.Message.Api.Presenters.Handlers.Base;

public class BaseHandlerPresenter :
    IUseCaseOutput,
    IUseCaseOutputInvalidInput,
    IUseCaseOutputHandlerError
{
    public void InvalidInput<TUseCaseInput>(TUseCaseInput input, NotificationsInputError errors)
        where TUseCaseInput : IUseCaseInput
    {
    }

    public void HandlerError<TUseCaseInput>(TUseCaseInput input, Exception error) where TUseCaseInput : IUseCaseInput
    {
    }
}