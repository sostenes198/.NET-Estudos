using Estudos.CleanArchitecture.Modular.Commons.Application.Events;
using Estudos.CleanArchitecture.Modular.Commons.Domain;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Domain.Signatures;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Domain.Signatures.Repositories;
using Estudos.CleanArchitecture.Modular.Modules.Signature.IntegratedEvents;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Infrastructure.Repositories.Signatures;

internal sealed class SignatureRepositoryAcl : ISignatureRepositoryAcl
{
    private readonly ICommandEventDispatcher<ResetSignatureNewApiInternalInternalCommandEvent, OperationResult> _commandEventDispatcherResetSignatureNewApi;

    public SignatureRepositoryAcl(ICommandEventDispatcher<ResetSignatureNewApiInternalInternalCommandEvent, OperationResult> commandEventDispatcherResetSignatureNewApi)
    {
        _commandEventDispatcherResetSignatureNewApi = commandEventDispatcherResetSignatureNewApi;
    }

    public Task<SignatureAcl> GetAsync(string account)
    {
        return Task.FromResult(new SignatureAcl(123321, account, new SignaturePassword("123", Guid.NewGuid()), false));
    }

    public Task<OperationResult> LegacyApiSignatureResetAsync(SignatureAcl signatureAcl)
    {
        return Task.FromResult(OperationResult.Success());
    }

    public Task<OperationResult> NewApiSignatureResetAsync(SignatureAcl signatureAcl)
    {
        return _commandEventDispatcherResetSignatureNewApi.DispatchAsync(new ResetSignatureNewApiInternalInternalCommandEvent(signatureAcl.Document, signatureAcl.Password.Password, signatureAcl.Password.GuidPassword));
    }
}