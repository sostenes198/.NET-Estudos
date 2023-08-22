using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.Validators;
using Estudos.CleanArchitecture.Modular.Modules.Signature.Domain.Signatures.Repositories;

namespace Estudos.CleanArchitecture.Modular.Modules.Signature.Application.UseCases.ResetSignature;

internal sealed class ResetSignatureUseCase : IUseCase<ResetSignatureUseCaseInput, IResetSignatureOutputUseCase>
{
    private readonly IUseCaseInputValidatorService<ResetSignatureUseCaseInput> _useCaseInputValidatorService;
    private readonly ISignatureRepository _signatureRepository;

    public ResetSignatureUseCase(IUseCaseInputValidatorService<ResetSignatureUseCaseInput> useCaseInputValidatorService,
        ISignatureRepository signatureRepository)
    {
        _useCaseInputValidatorService = useCaseInputValidatorService;
        _signatureRepository = signatureRepository;
    }

    public async Task ExecuteAsync(ResetSignatureUseCaseInput input, IResetSignatureOutputUseCase outputResult, CancellationToken cancellationToken = default)
    {
        if (!_useCaseInputValidatorService.Validate(input, out var notificationsInputError, cancellationToken))
        {
            outputResult.InvalidInput(input, notificationsInputError);

            return;
        }

        var signature = await _signatureRepository.GetAsync(input.Document);

        if (signature.IsEmpty)
        {
            outputResult.NotFound(input);

            return;
        }

        var operationResult = await _signatureRepository.ResetAsync(signature);

        if (operationResult.IsSuccess)
            outputResult.Success();
        else
            outputResult.FailedToResetSignature();
    }
}