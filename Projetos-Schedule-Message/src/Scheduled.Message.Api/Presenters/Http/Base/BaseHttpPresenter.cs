using Microsoft.AspNetCore.Mvc;
using Scheduled.Message.Api.Presenters.Base;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.Boundaries.UseCases.Outputs;
using Scheduled.Message.Application.Boundaries.UseCases.Validators;
#pragma warning disable CS9113 // Parameter is unread.

namespace Scheduled.Message.Api.Presenters.Http.Base;

public abstract class BaseHttpPresenter :
    IUseCaseOutput,
    IUseCaseOutputInvalidInput,
    IUseCaseOutputHandlerError
{
    public Func<IActionResult> Result { get; protected set; } = () => throw new NotImplementedException();

    public virtual void InvalidInput<TUseCaseInput>(TUseCaseInput input, NotificationsInputError errors)
        where TUseCaseInput : IUseCaseInput
    {
        Result = () => new BadRequestObjectResult(new NotificationErrorsResponse(errors.Errors));
    }

    public virtual void HandlerError<TUseCaseInput>(TUseCaseInput input, Exception error)
        where TUseCaseInput : IUseCaseInput
    {
        Result = () => new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}