namespace Scheduled.Message.Application.Boundaries.UseCases.Outputs;

public interface IUseCaseOutputHandlerError : IUseCaseOutput
{
    void HandlerError<TUseCaseInput>(TUseCaseInput input, Exception error)
        where TUseCaseInput : IUseCaseInput;
}