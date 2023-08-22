using Estudos.CleanArchitecture.Modular.Commons.Application.Events;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.Validators;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Domain.Signatures;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Domain.Signatures.Repositories;
using Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.IntegratedEvents;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.ACL.Application.UseCases.ResetSignatures;

internal sealed class ResetSignatureAclUseCase : IUseCase<ResetSignatureAclUseCaseInput, IResetSignatureAclOutputUseCase>
{
    private readonly IUseCaseInputValidatorService<ResetSignatureAclUseCaseInput> _useCaseInputValidatorService;
    private readonly IEventsDispatcher<MigrateSignatureInternalEvent> _eventsDispatcherMigrateSignature;
    private readonly ISignatureRepositoryAcl _signatureRepositoryAcl;

    public ResetSignatureAclUseCase(IUseCaseInputValidatorService<ResetSignatureAclUseCaseInput> useCaseInputValidatorService,
        IEventsDispatcher<MigrateSignatureInternalEvent> eventsDispatcherMigrateSignature,
        ISignatureRepositoryAcl signatureRepositoryAcl)
    {
        _useCaseInputValidatorService = useCaseInputValidatorService;
        _eventsDispatcherMigrateSignature = eventsDispatcherMigrateSignature;
        _signatureRepositoryAcl = signatureRepositoryAcl;
    }

    public async Task ExecuteAsync(ResetSignatureAclUseCaseInput input, IResetSignatureAclOutputUseCase outputResult, CancellationToken cancellationToken = default)
    {
        if (!_useCaseInputValidatorService.Validate(input, out var notificationsInputError, cancellationToken))
        {
            outputResult.InvalidInput(input, notificationsInputError);

            return;
        }

        var signature = await _signatureRepositoryAcl.GetAsync(input.Account);

        if (signature.IsEmpty)
        {
            outputResult.NotFound(input);

            return;
        }

        if (signature.IsMigrated)
        {
            await ResetInNewApiSignature(signature, outputResult);

            return;
        }

        if (signature.CanMigrate())
            await _eventsDispatcherMigrateSignature.DispatchAsync(new MigrateSignatureInternalEvent(signature.Document, signature.Password.Password, signature.Password.GuidPassword), cancellationToken);

        await ResetInLegacyApiSignature(signature, outputResult);
    }

    private async Task ResetInNewApiSignature(SignatureAcl signatureAcl, IResetSignatureAclOutputUseCase outputResult)
    {
        var operationResult = await _signatureRepositoryAcl.NewApiSignatureResetAsync(signatureAcl);

        if (operationResult.IsSuccess)
            outputResult.Success();
        else
            outputResult.FailedToResetSignature();
    }

    private async Task ResetInLegacyApiSignature(SignatureAcl signatureAcl, IResetSignatureAclOutputUseCase outputResult)
    {
        var operationResult = await _signatureRepositoryAcl.LegacyApiSignatureResetAsync(signatureAcl);

        if (operationResult.IsSuccess)
            outputResult.Success();
        else
            outputResult.FailedToResetSignature();
    }
}