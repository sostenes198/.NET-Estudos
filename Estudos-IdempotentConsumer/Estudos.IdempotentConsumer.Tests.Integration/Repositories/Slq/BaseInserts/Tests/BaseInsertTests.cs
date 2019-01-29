using System.Threading.Tasks;
using Estudos.IdempotentConsumer.Repositories.Slq.Base;
using Estudos.IdempotentConsumer.Tests.Integration.Helpers;
using Estudos.IdempotentConsumer.Tests.Integration.Repositories.Slq.Base;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Integration.Repositories.Slq.BaseInserts.Tests;

public class BaseInsertTests: BaseSqlRepositoryTest
{
    private readonly IDataContext _dataContext;
    
    public BaseInsertTests()
    {
        _dataContext = ServiceProvider.GetRequiredService<IDataContext>();
    }
    
    [Fact(DisplayName = "Deve ter inserido Entries padrão no banco de dados")]
    public async Task ShouldHaveEnteredDefaultEntriesInDabaseSuccessfully()
    {
        // arrange
        var expectedResult = BaseInsert.Entries;
        
        // act
        var result = await EntrySqlHelper.ListAsync(_dataContext);

        // assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}