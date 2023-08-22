namespace Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.Validators;

public interface IUseCaseInputValidatorService<in TInput>
    where TInput: UseCaseInput
{
    bool Validate(TInput input, out NotificationsInputError notificationsInputError, CancellationToken cancellationToken = default);
}