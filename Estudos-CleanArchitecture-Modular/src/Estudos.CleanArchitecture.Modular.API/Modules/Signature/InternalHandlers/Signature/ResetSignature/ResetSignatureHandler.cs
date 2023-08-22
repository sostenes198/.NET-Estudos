using Estudos.CleanArchitecture.Modular.API.Presenters.Modules.Signature.ResetSignature;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Commons.Domain;
using Estudos.CleanArchitecture.Modular.Commons.Events.Internal;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.ResetSignature;
using Estudos.CleanArchitecture.Modular.Modules.Signature.IntegratedEvents;

namespace Estudos.CleanArchitecture.Modular.API.Modules.Signature.InternalHandlers.Signature.ResetSignature;

public class ResetSignatureHandler : ICommandEventIInternalEventHandler<ResetSignatureNewApiInternalInternalCommandEvent, OperationResult>
{
    private readonly IUseCaseManager _useCaseManager;
    private readonly IResetSignatureOutputUseCase _resetSignatureOutputPresenter;

    public ResetSignatureHandler(IUseCaseManager useCaseManager,
        IResetSignatureOutputUseCase resetSignatureOutputPresenter)
    {
        _useCaseManager = useCaseManager;
        _resetSignatureOutputPresenter = resetSignatureOutputPresenter;
    }

    public async Task<OperationResult> Handle(ResetSignatureNewApiInternalInternalCommandEvent request, CancellationToken cancellationToken)
    {
        await _useCaseManager.ExecuteAsync(new ResetSignatureUseCaseInput(request.Document, request.Password, request.GuidPassword), _resetSignatureOutputPresenter, cancellationToken);

        return ((ResetSignatureOutputPresenter) _resetSignatureOutputPresenter).OperationResult();
    }
}