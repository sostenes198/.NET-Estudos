using Estudos.CleanArchitecture.Modular.API.Presenters.Modules.Signature.ACL.ResetSignature;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Application.UseCases.ResetSignatures;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.CleanArchitecture.Modular.API.Modules.Signature.ACL.Controllers;

[Route("api/v{version:apiVersion}/signature-acl")]
public class SignatureAclController : ControllerBase
{
    private readonly IUseCaseManager _useCaseManager;

    public SignatureAclController(ILogger<SignatureAclController> logger,
        IUseCaseManager useCaseManager)
        : base(logger)
    {
        _useCaseManager = useCaseManager;
    }

    [HttpPost("reset/signature")]
    public async Task<IActionResult> ResetSignature([FromServices] IResetSignatureAclOutputUseCase presenter, [FromBody] ResetSignatureAclRequest request, CancellationToken cancellationToken)
    {
        await _useCaseManager.ExecuteAsync(new ResetSignatureAclUseCaseInput(request.GuidPassword, request.Document, request.Account, request.Password), presenter, cancellationToken);

        return ((ResetSignatureAclOutputPresenter) presenter).Result();
    }
}