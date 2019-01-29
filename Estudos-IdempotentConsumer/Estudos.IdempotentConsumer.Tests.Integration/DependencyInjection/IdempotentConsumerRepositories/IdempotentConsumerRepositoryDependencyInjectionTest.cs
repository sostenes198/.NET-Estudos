using Estudos.IdempotentConsumer.Base;
using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Consumer.Repository;
using Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;
using Estudos.IdempotentConsumer.Options;
using Estudos.IdempotentConsumer.Repositories.Base;
using Estudos.IdempotentConsumer.Repositories.CircularBuffer;
using Estudos.IdempotentConsumer.Repositories.Slq;
using Estudos.IdempotentConsumer.Repositories.Slq.Base;
using Estudos.IdempotentConsumer.Strategies.Repository;
using Estudos.IdempotentConsumer.Tests.Integration.Fixtures;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Integration.DependencyInjection.IdempotentConsumerRepositories;

public class IdempotentConsumerRepositoryDependencyInjectionTest : BaseTest
{
    public IdempotentConsumerRepositoryDependencyInjectionTest()
    {
        ConfigureServices += (context, collection) =>
        {
            collection.AddIdempotentConsumerRepository(IdempotentConsumerRepositoryOptionsFixture.Options(context.Configuration));
        };
    }

    [Fact(DisplayName = "Deve validar injeção de dependência AddIdempotentConsumerRepository")]
    public void ShouldValidateDependencyInjectionAddIdempotentConsumerRepository()
    {
        // arrange - act
        var idempotentConsumerRepository = ServiceProvider.GetRequiredService<IIdempotentConsumerRepository>();
        var membershipStrategy =  ServiceProvider.GetRequiredService<IMembershipStrategy<DefaultIdempotentConsumerKey>>();
        var repository =  ServiceProvider.GetRequiredService<IRepository>();
        var inMemoryCircularBufferRepository =  ServiceProvider.GetRequiredService<IInMemoryCircularBufferRepository>();
        var sqlService =  ServiceProvider.GetRequiredService<ISqlService>();
        var dataContext =  ServiceProvider.GetRequiredService<IDataContext>();
        var sqlRepositoryOptions =  ServiceProvider.GetRequiredService<IOptions<SqlRepositoryOptions>>();
        var inMemoryCircularBufferRepositoryOptions =  ServiceProvider.GetRequiredService<IOptions<InMemoryCircularBufferRepositoryOptions>>();
        
        // assert
        idempotentConsumerRepository.Should().NotBeNull().And.BeOfType<IdempotentConsumerRepository>();
        membershipStrategy.Should().NotBeNull().And.BeOfType<RepositoryMembershipStrategy<DefaultIdempotentConsumerKey>>();
        repository.Should().NotBeNull().And.BeOfType<SqlServerRepository>();
        inMemoryCircularBufferRepository.Should().NotBeNull().And.BeOfType<InMemoryCircularBufferRepository>();
        sqlService.Should().NotBeNull().And.BeOfType<SqlService>();
        dataContext.Should().NotBeNull().And.BeOfType<DataContext>();
        sqlRepositoryOptions.Should().NotBeNull();
        inMemoryCircularBufferRepositoryOptions.Should();
    }
}