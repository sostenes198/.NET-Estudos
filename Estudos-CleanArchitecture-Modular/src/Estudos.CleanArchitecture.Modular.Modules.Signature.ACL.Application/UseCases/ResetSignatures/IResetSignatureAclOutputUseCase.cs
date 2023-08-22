using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.UseCaseResults;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Application.UseCases.ResetSignatures;

public interface IResetSignatureAclOutputUseCase : IOutputUseCaseResult,
    IOutputUseCaseResultInvalidInput<ResetSignatureAclUseCaseInput>,
    IOutputUseCaseResultNotFound<ResetSignatureAclUseCaseInput>
{
    void Success();

    void FailedToMigrate();

    void FailedToResetSignature();
}