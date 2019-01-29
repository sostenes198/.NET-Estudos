using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.IdempotentConsumer.Base;
using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Consumer.Repository;
using Estudos.IdempotentConsumer.DependencyInjection;
using Estudos.IdempotentConsumer.Enums;
using Estudos.IdempotentConsumer.Repositories.Base;
using Estudos.IdempotentConsumer.Repositories.CircularBuffer;
using Estudos.IdempotentConsumer.Repositories.Slq;
using Estudos.IdempotentConsumer.Repositories.Slq.Base;
using Estudos.IdempotentConsumer.Strategies.Repository;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.DependencyInjection;

public class IdempotentConsumerDependencyInjectionTest
{
    private readonly IServiceCollection _serviceCollection;

    public IdempotentConsumerDependencyInjectionTest()
    {
        _serviceCollection = new ServiceCollection();
    }

    [Fact(DisplayName = "Deve adicionar Idempotent Consumer padrão")]
    public void ShouldAddDefaultIdempotentConsumer()
    {
        // arrange - act
        _serviceCollection.AddIdempotentConsumer<IdempotentConsumerDependencyInjectionFixture.DefaultIdempotentConsumer>();

        var provider = _serviceCollection.BuildServiceProvider();
        var idempotentConsumer = provider.GetRequiredService<IIdempotentConsumer<DefaultIdempotentConsumerKey>>();

        // assert
        idempotentConsumer.Should().NotBeNull().And.BeOfType<IdempotentConsumerDependencyInjectionFixture.DefaultIdempotentConsumer>();
    }

    [Fact(DisplayName = "Deve adicionar Idempotent Consumer")]
    public void ShouldAddIdempotentConsumer()
    {
        // arrange - act
        _serviceCollection.AddIdempotentConsumer<IdempotentConsumerRepository>(services => { services.TryAddSingleton(_ => new Mock<IMembershipStrategy<DefaultIdempotentConsumerKey>>().Object); });

        var provider = _serviceCollection.BuildServiceProvider();
        var idempotentConsumer = provider.GetRequiredService<IIdempotentConsumer<DefaultIdempotentConsumerKey>>();

        // assert
        idempotentConsumer.Should().NotBeNull().And.BeOfType<IdempotentConsumerRepository>();
    }

    [Fact(DisplayName = "Deve lançar exceção quando idempotent consumer inválido")]
    public void ShouldThrownExceptionWhenInvalidIdempotentConsumer()
    {
        // arrange - act
        var exceptionAssertions = _serviceCollection.Invoking(lnq => lnq.AddIdempotentConsumer<IdempotentConsumerDependencyInjectionFixture.InvalidIdempotentConsumer>()).Should().ThrowExactly<ArgumentException>();

        // assert
        exceptionAssertions.WithMessage("InvalidIdempotentConsumer should inherit from IIdempotentConsumer`1");
    }

    [Fact(DisplayName = "Deve adicionar MembershipStrategy padrão")]
    public void ShouldAddDefaultMembershipStrategy()
    {
        // arrange - act
        _serviceCollection.AddIdempotentConsumerMembershipStrategy<IdempotentConsumerDependencyInjectionFixture.DefaultMembershipStrategy>();

        var provider = _serviceCollection.BuildServiceProvider();
        var membershipStrategy = provider.GetRequiredService<IMembershipStrategy<DefaultIdempotentConsumerKey>>();

        // assert
        membershipStrategy.Should().NotBeNull().And.BeOfType<IdempotentConsumerDependencyInjectionFixture.DefaultMembershipStrategy>();
    }

    [Fact(DisplayName = "Deve adicionar MembershipStrategy")]
    public void ShouldAddMembershipStrategy()
    {
        // arrange - act
        _serviceCollection.AddIdempotentConsumerMembershipStrategy<RepositoryMembershipStrategy<DefaultIdempotentConsumerKey>>(services => { services.TryAddSingleton(_ => new Mock<IRepository>().Object); });

        var provider = _serviceCollection.BuildServiceProvider();
        var membershipStrategy = provider.GetRequiredService<IMembershipStrategy<DefaultIdempotentConsumerKey>>();

        // assert
        membershipStrategy.Should().NotBeNull().And.BeOfType<RepositoryMembershipStrategy<DefaultIdempotentConsumerKey>>();
    }

    [Fact(DisplayName = "Deve lançar exceção quando MembershipStrategy inválido")]
    public void ShouldThrownExceptionWhenInvalidMembershipStrategy()
    {
        // arrange - act
        var exceptionAssertions = _serviceCollection.Invoking(lnq => lnq.AddIdempotentConsumerMembershipStrategy<IdempotentConsumerDependencyInjectionFixture.InvalidMembershipStrategy>()).Should().ThrowExactly<ArgumentException>();

        // assert
        exceptionAssertions.WithMessage("InvalidMembershipStrategy should inherit from IMembershipStrategy`1");
    }

    [Fact(DisplayName = "Deve adicionar Repositório padrão")]
    public void ShouldAddDefaultRepository()
    {
        // arrange - act
        _serviceCollection.AddIdempotentConsumerRepository<IdempotentConsumerDependencyInjectionFixture.DefaultRepository>(ServiceLifetime.Singleton);

        var provider = _serviceCollection.BuildServiceProvider();
        var repository = provider.GetRequiredService<IRepository>();

        // assert
        repository.Should().NotBeNull().And.BeOfType<IdempotentConsumerDependencyInjectionFixture.DefaultRepository>();
    }

    [Fact(DisplayName = "Deve adicionar Repositório")]
    public void ShouldAddRepository()
    {
        // arrange - act
        _serviceCollection.AddIdempotentConsumerRepository<SqlServerRepository>(ServiceLifetime.Scoped, services =>
        {
            services.TryAddSingleton(_ => new Mock<ISqlService>().Object);
            services.TryAddSingleton(_ => new Mock<IInMemoryCircularBufferRepository>().Object);
        });

        var provider = _serviceCollection.BuildServiceProvider();
        var repository = provider.GetRequiredService<IRepository>();

        // assert
        repository.Should().NotBeNull().And.BeOfType<SqlServerRepository>();
    }
}

public class IdempotentConsumerDependencyInjectionFixture
{
    public class DefaultIdempotentConsumer : IIdempotentConsumer<DefaultIdempotentConsumerKey>
    {
        public Task<CompletionResult<TResult?>> ProcessAsync<TResult>(string instanceId, DefaultIdempotentConsumerKey message, Func<Task<TResult?>> funcProcess)
        {
            throw new NotImplementedException();
        }

        public Task<CompletionStatus> ProcessAsync(string instanceId, DefaultIdempotentConsumerKey message, Func<Task> funcProcess)
        {
            throw new NotImplementedException();
        }
    }

    public class InvalidIdempotentConsumer : IIdempotentConsumer
    {
    }

    public class DefaultMembershipStrategy : IMembershipStrategy<DefaultIdempotentConsumerKey>
    {
        public Task<bool> ExistsAsync(string instanceId, DefaultIdempotentConsumerKey message)
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(string instanceId, DefaultIdempotentConsumerKey message)
        {
            throw new NotImplementedException();
        }
    }

    public class InvalidMembershipStrategy : IMembershipStrategy
    {
    }

    public class DefaultRepository : IRepository
    {
        public Task<bool> ContainsAsync(string instanceId, string idempotencyKey)
        {
            throw new NotImplementedException();
        }

        public Task<Entry> GetEntryAsync(string instanceId, string idempotencyKey)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Entry>> GetEntriesAsync(string instanceId, int dataFetchThreshold)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdateAsync(Entry data)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string instanceId, string idempotencyKey)
        {
            throw new NotImplementedException();
        }
    }
}