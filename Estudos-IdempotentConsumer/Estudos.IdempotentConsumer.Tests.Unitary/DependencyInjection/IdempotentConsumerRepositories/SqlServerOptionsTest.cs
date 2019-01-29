using System;
using Estudos.IdempotentConsumer.DependencyInjection.IdempotentConsumerRepositories;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.DependencyInjection.IdempotentConsumerRepositories;

public class SqlServerOptionsTest
{
    [Fact(DisplayName = "Deve criar SqlServerOptions")]
    public void ShouldCreateSqlServerOptions()
    {
        // arrange - act
        var options = new SqlServerOptions("ConnectionString");
        
        // assert
        options.ConnectionString.Should().BeEquivalentTo("ConnectionString");
    }

    [Fact(DisplayName = "Deve lançar exceção ao tentar criar SqlServerOptions e connection string inválida")]
    public void ShouldThrownExceptionWhenTryCreateSqlServerOptionsAndInvalidConnectionString()
    {
        // arrange - act
        // ReSharper disable once ObjectCreationAsStatement
        Action act = () => new SqlServerOptions(string.Empty);

        var exceptionAssertions = act.Should().ThrowExactly<ArgumentNullException>();

        // assert
        exceptionAssertions.WithMessage("Value cannot be null. (Parameter 'connectionString')");
    }
}