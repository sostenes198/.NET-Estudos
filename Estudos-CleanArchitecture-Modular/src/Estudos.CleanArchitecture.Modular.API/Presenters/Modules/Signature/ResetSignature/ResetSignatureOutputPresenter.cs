using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.Validators;
using Estudos.CleanArchitecture.Modular.Commons.Domain;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.ResetSignature;

namespace Estudos.CleanArchitecture.Modular.API.Presenters.Modules.Signature.ResetSignature;

public sealed class ResetSignatureOutputPresenter : IResetSignatureOutputUseCase
{
    public Func<OperationResult> OperationResult { get; private set; }

    public ResetSignatureOutputPresenter()
    {
        OperationResult = () => throw new NotImplementedException();
    }
    
    public void Success()
    {
        OperationResult = Commons.Domain.OperationResult.Success;
    }

    public void FailedToResetSignature()
    {
        OperationResult = Commons.Domain.OperationResult.Fail;
    }

    public void InvalidInput(ResetSignatureUseCaseInput input, NotificationsInputError notificationsInputError)
    {
        OperationResult = Commons.Domain.OperationResult.Fail;
    }

    public void NotFound(ResetSignatureUseCaseInput input)
    {
        OperationResult = Commons.Domain.OperationResult.Fail;
    }
}