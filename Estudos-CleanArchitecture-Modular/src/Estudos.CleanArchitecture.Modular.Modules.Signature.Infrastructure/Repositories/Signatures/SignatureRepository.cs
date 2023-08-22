using Estudos.CleanArchitecture.Modular.Commons.Domain;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Domain.Signatures;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Domain.Signatures.Repositories;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.Infrastructure.Repositories.Signatures;

internal sealed class SignatureRepository : ISignatureRepository
{
    public Task<Domain.Signatures.Signature> GetAsync(long document)
    {
        return Task.FromResult(new Domain.Signatures.Signature(document, new SignaturePassword("123", Guid.NewGuid())));
    }

    public Task<OperationResult> SaveAsync(Domain.Signatures.Signature signature)
    {
        return Task.FromResult(OperationResult.Success());
    }

    public Task<OperationResult> ResetAsync(Domain.Signatures.Signature signature)
    {
        return Task.FromResult(OperationResult.Success());
    }
}