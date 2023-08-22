using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.UseCaseResults;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.MigrateSignature;

public interface IMigrateSignatureOutputUseCase : IOutputUseCaseResult,
    IOutputUseCaseResultInvalidInput<MigrateSignatureUseCaseInput>
{
    void Success();

    void FailedToResetSignature();
}