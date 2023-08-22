namespace Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.UseCaseResults;

public interface IOutputUseCaseResultNotFound<in TUseCaseInput> 
    where TUseCaseInput : UseCaseInput
{
    void NotFound(TUseCaseInput input);
}