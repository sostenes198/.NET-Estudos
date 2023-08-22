using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.UseCaseResults;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.Validators;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.CleanArchitecture.Modular.API.Presenters.Base;

public class BasePresenterHttpActionResult<TUseCaseInput> : IOutputUseCaseResult,
    IOutputUseCaseResultInvalidInput<TUseCaseInput>,
    IOutputUseCaseResultNotFound<TUseCaseInput>,
    IOutputUseCaseResultInternalError<TUseCaseInput>
    where TUseCaseInput : UseCaseInput
{
    public Func<IActionResult> Result { get; protected set; }

    protected BasePresenterHttpActionResult()
    {
        Result = () => throw new NotImplementedException();
    }

    public void InvalidInput(TUseCaseInput input, NotificationsInputError notificationsInputError)
    {
        Result = () => new BadRequestObjectResult(new NotificationErrorsResponse(notificationsInputError.Errors));
    }

    public void NotFound(TUseCaseInput input)
    {
        Result = () => new NotFoundResult();
    }

    public void InternalError(TUseCaseInput input)
    {
        Result = () => new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}