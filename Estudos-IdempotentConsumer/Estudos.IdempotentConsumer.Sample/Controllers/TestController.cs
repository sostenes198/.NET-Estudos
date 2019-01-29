using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Consumer.Repository;
using Estudos.IdempotentConsumer.Sample.Application;
using Estudos.IdempotentConsumer.Sample.Application.UseCase;
using Microsoft.AspNetCore.Mvc;

namespace Estudos.IdempotentConsumer.Sample.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IIdempotentConsumerRepository _idempotentConsumerRepository;
    private readonly IUseCase<Return> _useCaseWithReturn;
    private readonly IUseCase _useCaseWithoutReturn;

    public TestController(IIdempotentConsumerRepository idempotentConsumerRepository, IUseCase<Return> useCaseWithReturn, IUseCase useCaseWithoutReturn)
    {
        _idempotentConsumerRepository = idempotentConsumerRepository;
        _useCaseWithReturn = useCaseWithReturn;
        _useCaseWithoutReturn = useCaseWithoutReturn;
    }

    [HttpGet("{transactionId}/usecase-with-return")]
    public async Task<IActionResult> UseCaseWithReturnAsync(Guid transactionId)
    {
        var result = await _idempotentConsumerRepository.ProcessAsync("usecase-with-return", new DefaultIdempotentConsumerKey(transactionId.ToString()), () => _useCaseWithReturn.ExecuteAsync()!);
        return new OkObjectResult(new
        {
            Result = result.Result,
            CompletionStatus = result.CompletionStatus.ToString()
        });
    }

    [HttpGet("{transactionId}/usecase-without-return")]
    public async Task<IActionResult> UseCaseWithoutReturnAsync(Guid transactionId)
    {
        var result = await _idempotentConsumerRepository.ProcessAsync("usecase-without-return", new DefaultIdempotentConsumerKey(transactionId.ToString()), () => _useCaseWithoutReturn.ExecuteAsync());
        return new OkObjectResult(result.ToString());
    }
}