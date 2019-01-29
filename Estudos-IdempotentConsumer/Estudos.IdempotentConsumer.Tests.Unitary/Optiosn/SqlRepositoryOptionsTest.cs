using Estudos.IdempotentConsumer.Options;
using FluentAssertions;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Optiosn;

public class SqlRepositoryOptionsTest
{
    [Fact(DisplayName = "Deve criar objeto SqlRepositoryOptions")]
    public void ShouldCreateSqlRepositoryOptionsObject()
    {
        // arrange - act
        var sqlRepositoryOptions = new SqlRepositoryOptions
        {
            ConnectionString = "ConnectionString"
        };

        // assert
        sqlRepositoryOptions.ConnectionString.Should().BeEquivalentTo("ConnectionString");
    }
}