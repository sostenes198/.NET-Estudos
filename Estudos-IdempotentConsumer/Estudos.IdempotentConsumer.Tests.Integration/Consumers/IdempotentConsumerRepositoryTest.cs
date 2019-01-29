using System.Threading.Tasks;
using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Consumer.Repository;
using Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;
using Estudos.IdempotentConsumer.Enums;
using Estudos.IdempotentConsumer.Repositories.Slq.Base;
using Estudos.IdempotentConsumer.Tests.Integration.Fixtures;
using Estudos.IdempotentConsumer.Tests.Integration.Helpers;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Integration.Consumers;

public class IdempotentConsumerRepositoryTest : BaseTest
{
    private const string InstanceId = "IdempotentConsumerRepository-IntegrationTest-123";
    private const string IdempotencyKey1 = "1381600f-965d-41fa-b95a-3d7cf92068c6";
    private const string IdempotencyKey2 = "64fd8e92-0dd5-47fa-bea9-97b606a4a0b7";

    private readonly IIdempotentConsumerRepository _idempotentConsumerRepository;
    private readonly IDataContext _dataContext;

    public IdempotentConsumerRepositoryTest()
    {
        ConfigureServices += (context, collection) => { collection.AddIdempotentConsumerRepository(IdempotentConsumerRepositoryOptionsFixture.Options(context.Configuration)); };

        _idempotentConsumerRepository = ServiceProvider.GetRequiredService<IIdempotentConsumerRepository>();
        _dataContext = ServiceProvider.GetRequiredService<IDataContext>();
    }

    [Fact(DisplayName = "Deve processar mensagem")]
    public async Task ShouldProcessMessage()
    {
        // arrange
        await CleanToTestAsync();

        // act
        var result = await _idempotentConsumerRepository.ProcessAsync(InstanceId, new DefaultIdempotentConsumerKey(IdempotencyKey1), () => Task.CompletedTask);

        // assert
        result.Should().Be(CompletionStatus.Consumed);
    }

    [Fact(DisplayName = "Deve ignorar mensagem quando já já estiver sido processada")]
    public async Task ShouldIgnoreMessageWhenItHasAlreadyBeenProcessed()
    {
        // arrange
        await CleanToTestAsync();

        await _idempotentConsumerRepository.ProcessAsync(InstanceId, new DefaultIdempotentConsumerKey(IdempotencyKey1), () => Task.CompletedTask);

        // act
        var result = await _idempotentConsumerRepository.ProcessAsync(InstanceId, new DefaultIdempotentConsumerKey(IdempotencyKey1), () => Task.CompletedTask);

        // assert
        result.Should().Be(CompletionStatus.Ignored);
    }

    [Fact(DisplayName = "Deve testar concorrência e ignorar mensagem quando a primeira já estiver em processamento")]
    public async Task ShouldValidateConcurrencyAndIgnoreMessageWhenFirstOneIsAlreadyProcessing()
    {
        // arrange
        await CleanToTestAsync();

        _ = _idempotentConsumerRepository.ProcessAsync(InstanceId, new DefaultIdempotentConsumerKey(IdempotencyKey1), () => Task.CompletedTask);

        // act
        _ = _idempotentConsumerRepository.ProcessAsync(InstanceId, new DefaultIdempotentConsumerKey(IdempotencyKey2), () => Task.Delay(1000));
        await Task.Delay(100);
        var result = await _idempotentConsumerRepository.ProcessAsync(InstanceId, new DefaultIdempotentConsumerKey(IdempotencyKey2), () => Task.CompletedTask);

        // assert
        result.Should().Be(CompletionStatus.Ignored);
    }

    private Task CleanToTestAsync()
    {
        return Task.WhenAll(EntrySqlHelper.DeleteAsync(_dataContext, InstanceId, IdempotencyKey1),
                            EntrySqlHelper.DeleteAsync(_dataContext, InstanceId, IdempotencyKey2));
    }

    protected override void DisposeBase()
    {
        Task.Run(async () => await CleanToTestAsync()).Wait();
    }
}