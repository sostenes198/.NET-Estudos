using System.Collections.Generic;
using System.Threading.Tasks;
using Estudos.IdempotentConsumer.Base;
using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Enums;
using Estudos.IdempotentConsumer.Repositories.Base;
using Estudos.IdempotentConsumer.Strategies.Repository;
using FluentAssertions;
using Moq;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Strategies.Repository;

public class RepositoryMembershipStrategyTest
{
    private const string InstanceId = "InstanceId-1";
    private const string IdempotencyKey = "Key";

    private readonly DefaultIdempotentConsumerKey _defaultIdempotentConsumer = new(IdempotencyKey);
    private readonly Mock<IRepository> _repositoryMock;
    private readonly IMembershipStrategyExtended<DefaultIdempotentConsumerKey> _membershipStrategyExtended;

    public RepositoryMembershipStrategyTest()
    {
        _repositoryMock = new Mock<IRepository>();
        _membershipStrategyExtended = new RepositoryMembershipStrategy<DefaultIdempotentConsumerKey>(_repositoryMock.Object);
    }

    [Theory(DisplayName = "Deve validar se item exist repositório")]
    [MemberData(nameof(ScenariosExistItem))]
    public async Task ShouldValidateItemExistRepository(bool containsResultRepository, bool expectedResult, int expectedCountCallAddOrUpdateRepository)
    {
        // arrange
        _repositoryMock.Setup(lnq => lnq.ContainsAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(containsResultRepository);

        // act
        var result = await _membershipStrategyExtended.ExistsAsync(InstanceId, _defaultIdempotentConsumer);

        // assert
        result.Should().Be(expectedResult);
        _repositoryMock.Verify(lnq => lnq.ContainsAsync(InstanceId, IdempotencyKey), Times.Once);
        _repositoryMock.Verify(lnq => lnq.AddOrUpdateAsync(It.Is<Entry>(t => t.InstanceId == InstanceId && t.IdempotencyKey == IdempotencyKey)), Times.Exactly(expectedCountCallAddOrUpdateRepository));
    }

    [Fact(DisplayName = "Deve adicionar item repositório ")]
    public async Task ShouldAddItemRepository()
    {
        // arrange - act
        await _membershipStrategyExtended.AddAsync(InstanceId, _defaultIdempotentConsumer);

        // assert
        _repositoryMock.Verify(lnq => lnq.AddOrUpdateAsync(It.Is<Entry>(t => t.InstanceId == InstanceId && t.IdempotencyKey == IdempotencyKey)), Times.Once);
    }

    [Fact(DisplayName = "Deve remover item repositório")]
    public async Task ShouldRemoveItemRepository()
    {
        // arrange - act
        await _membershipStrategyExtended.RemoveAsync(InstanceId, _defaultIdempotentConsumer);

        // assert
        _repositoryMock.Verify(lnq => lnq.RemoveAsync(InstanceId, IdempotencyKey), Times.Once);
    }

    [Fact(DisplayName = "Deve setar estado do item repositório")]
    public void ShouldSetStateItemRepository()
    {
        // arrange - act
        var state = RepositoryEntryState.None;
        _membershipStrategyExtended.SetStateAsync(InstanceId, _defaultIdempotentConsumer, state);

        // assert
        _repositoryMock.Verify(lnq => lnq.AddOrUpdateAsync(It.Is<Entry>(t => t.InstanceId == InstanceId && t.IdempotencyKey == IdempotencyKey && t.State == state)));
    }

    public static IEnumerable<object[]> ScenariosExistItem => new List<object[]>
    {
        new object[] {true, true, 0},
        new object[] {false, false, 1}
    };
}