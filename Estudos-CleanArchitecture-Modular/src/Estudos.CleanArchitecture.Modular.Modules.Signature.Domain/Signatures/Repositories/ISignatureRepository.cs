using Estudos.CleanArchitecture.Modular.Commons.Domain;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.Domain.Signatures.Repositories;

public interface ISignatureRepository
{
    Task<Signature> GetAsync(long document);

    Task<OperationResult> SaveAsync(Signature signature);

    Task<OperationResult> ResetAsync(Signature signature);
}