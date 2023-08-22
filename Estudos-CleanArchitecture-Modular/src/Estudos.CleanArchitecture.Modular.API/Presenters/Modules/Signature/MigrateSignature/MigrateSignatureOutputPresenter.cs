using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.Validators;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.MigrateSignature;

namespace Estudos.CleanArchitecture.Modular.API.Presenters.Modules.Signature.MigrateSignature;

public sealed class MigrateSignatureOutputPresenter : IMigrateSignatureOutputUseCase
{
    public void InvalidInput(MigrateSignatureUseCaseInput input, NotificationsInputError notificationsInputError)
    {
    }

    public void Success()
    {
    }

    public void FailedToResetSignature()
    {
    }
}