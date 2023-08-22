using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Commons.Events.Internal;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.IntegratedEvents;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.MigrateSignature;

namespace Estudos.CleanArchitecture.Modular.API.Modules.Signature.InternalHandlers.Signature.MigrateSignature;

public class MigrateSignatureHandler : IInternalEventHandler<MigrateSignatureInternalEvent>
{
    private readonly IUseCaseManager _useCaseManager;
    private readonly IMigrateSignatureOutputUseCase _migrateSignatureOutputPresenter;

    public MigrateSignatureHandler(IUseCaseManager useCaseManager,
        IMigrateSignatureOutputUseCase migrateSignatureOutputPresenter)
    {
        _useCaseManager = useCaseManager;
        _migrateSignatureOutputPresenter = migrateSignatureOutputPresenter;
    }

    public async Task Handle(MigrateSignatureInternalEvent request, CancellationToken cancellationToken)
    {
        await _useCaseManager.ExecuteAsync(new MigrateSignatureUseCaseInput(request.Document, request.Password, request.GuidPassword), _migrateSignatureOutputPresenter, cancellationToken);
    }
}