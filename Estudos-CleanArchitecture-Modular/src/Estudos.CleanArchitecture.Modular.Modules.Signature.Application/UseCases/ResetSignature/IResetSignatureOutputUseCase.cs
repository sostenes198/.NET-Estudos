using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.UseCaseResults;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.ResetSignature;

public interface IResetSignatureOutputUseCase : IOutputUseCaseResult,
    IOutputUseCaseResultInvalidInput<ResetSignatureUseCaseInput>,
    IOutputUseCaseResultNotFound<ResetSignatureUseCaseInput>
{
    void Success();

    void FailedToResetSignature();
}