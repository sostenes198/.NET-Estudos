using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.Validators;

namespace Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.UseCaseResults;

public interface IOutputUseCaseResultInvalidInput<in TUseCaseInput>
    where TUseCaseInput : UseCaseInput
{
    void InvalidInput(TUseCaseInput input, NotificationsInputError notificationsInputError);
}