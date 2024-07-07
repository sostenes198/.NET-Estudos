namespace Scheduled.Message.Application.Boundaries.UseCases.Validators;

public interface IUseCaseInputValidator<in TInput>
    where TInput : IUseCaseInput
{
    bool Validate(TInput input, out NotificationsInputError notificationErrors, CancellationToken token = default);
}