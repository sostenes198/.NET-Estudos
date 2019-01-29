using System;
using Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.DependencyInjection.IdempotentConsumerRepositories;

public class IdempotentConsumerRepositoryOptionsTest
{
    [Fact(DisplayName = "Deve criar objeto IdempotentConsumerRepositoryOptions")]
    public void ShouldCreateObjectIdempotentConsumerRepositoryOptions()
    {
        // arrange - act
        var result = new IdempotentConsumerRepositoryOptions(new SqlServerOptions("Test"), new InMemoryRepositoryOptions(100));

        // assert
        result.SqlServerOptions.Should().NotBeNull();
        result.InMemoryRepositoryOptions.Should().NotBeNull();
    }

    [Fact(DisplayName = "Deve falhar quando SqlServerOptions for nullo ao criar IdempotentConsumerRepositoryOptions")]
    public void ShouldThrownExceptionWhenSqlServerOptionsIsNullWhenTryToCreateIdempotentConsumerRepositoryOptions()
    {
        // arrange
        // ReSharper disable once ObjectCreationAsStatement
        Action act = () => new IdempotentConsumerRepositoryOptions(null!, new InMemoryRepositoryOptions());
        
        //  act
        var exceptionAssertions = act.Should().ThrowExactly<ArgumentNullException>();


        // assert
        exceptionAssertions.WithMessage("Value cannot be null. (Parameter 'sqlServerOptions')");
    }
    
    [Fact(DisplayName = "Deve falhar quando InMemoryRepositoryOptions for nullo ao criar IdempotentConsumerRepositoryOptions")]
    public void ShouldThrownExceptionWhenInMemoryRepositoryOptionsIsNullWhenTryToCreateIdempotentConsumerRepositoryOptions()
    {
        // arrange
        // ReSharper disable once ObjectCreationAsStatement
        Action act = () => new IdempotentConsumerRepositoryOptions(new SqlServerOptions("Test"), null!);
        
        //  act
        var exceptionAssertions = act.Should().ThrowExactly<ArgumentNullException>();


        // assert
        exceptionAssertions.WithMessage("Value cannot be null. (Parameter 'inMemoryRepositoryOptions')");
    }
}