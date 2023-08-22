using Estudos.CleanArchitecture.Modular.API.Presenters.Base;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Application.UseCases.ResetSignatures;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.CleanArchitecture.Modular.API.Presenters.Modules.Signature.ACL.ResetSignature;

public class ResetSignatureAclOutputPresenter : BasePresenterHttpActionResult<ResetSignatureAclUseCaseInput>, IResetSignatureAclOutputUseCase
{
    public void Success()
    {
        Result = () => new OkResult();
    }

    public void FailedToMigrate()
    {
        Result = () => new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }

    public void FailedToResetSignature()
    {
        Result = () => new StatusCodeResult(StatusCodes.Status500InternalServerError);
    }
}