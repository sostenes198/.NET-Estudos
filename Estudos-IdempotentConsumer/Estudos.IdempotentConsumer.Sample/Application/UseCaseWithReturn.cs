using Estudos.IdempotentConsumer.Sample.Application.UseCase;

namespace Estudos.IdempotentConsumer.Sample.Application;

public class UseCaseWithReturn : IUseCase<Return>
{
    public Task<Return> ExecuteAsync()
    {
        return Task.FromResult(new Return(10, "Deu bom"));
    }
}

public class Return
{
    public int Id { get;  }
    public string Message { get; }
    
    public Return(int id, string message)
    {
        Id = id;
        Message = message;
    }
}