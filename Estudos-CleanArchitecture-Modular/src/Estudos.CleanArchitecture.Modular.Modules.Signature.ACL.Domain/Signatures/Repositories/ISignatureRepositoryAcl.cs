using Estudos.CleanArchitecture.Modular.Commons.Domain;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Domain.Signatures.Repositories;

public interface ISignatureRepositoryAcl
{
    Task<SignatureAcl> GetAsync(string account);

    Task<OperationResult> LegacyApiSignatureResetAsync(SignatureAcl signatureAcl);

    Task<OperationResult> NewApiSignatureResetAsync(SignatureAcl signatureAcl);
}