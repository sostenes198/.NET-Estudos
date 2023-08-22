using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.Validators;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Domain.Signatures;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Domain.Signatures.Repositories;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.MigrateSignature;

internal sealed class MigrateSignatureUseCase : IUseCase<MigrateSignatureUseCaseInput, IMigrateSignatureOutputUseCase>
{
    private readonly IUseCaseInputValidatorService<MigrateSignatureUseCaseInput> _useCaseInputValidatorService;
    private readonly ISignatureRepository _signatureRepository;

    public MigrateSignatureUseCase(IUseCaseInputValidatorService<MigrateSignatureUseCaseInput> useCaseInputValidatorService,
        ISignatureRepository signatureRepository)
    {
        _useCaseInputValidatorService = useCaseInputValidatorService;
        _signatureRepository = signatureRepository;
    }

    public async Task ExecuteAsync(MigrateSignatureUseCaseInput input, IMigrateSignatureOutputUseCase outputResult, CancellationToken cancellationToken = default)
    {
        if (!_useCaseInputValidatorService.Validate(input, out var notificationsInputError, cancellationToken))
        {
            outputResult.InvalidInput(input, notificationsInputError);

            return;
        }

        var signature = new Domain.Signatures.Signature(input.Document, new SignaturePassword(input.Password, input.GuidPassword));

        var operationResult = await _signatureRepository.SaveAsync(signature);

        if (operationResult.IsSuccess)
            outputResult.Success();
        else
            outputResult.FailedToResetSignature();
    }
}