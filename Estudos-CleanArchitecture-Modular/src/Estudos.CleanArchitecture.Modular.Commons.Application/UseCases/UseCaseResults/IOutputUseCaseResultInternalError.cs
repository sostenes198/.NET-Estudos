namespace Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.UseCaseResults;

public interface IOutputUseCaseResultInternalError<in TUseCaseInput> 
    where TUseCaseInput : UseCaseInput
{
    void InternalError(TUseCaseInput input);
}