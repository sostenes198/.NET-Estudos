using System;
using System.Threading.Tasks;
using Estudos.IdempotentConsumer.Base;
using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Enums;
using FluentAssertions;
using Moq;
using Xunit;

namespace Estudos.IdempotentConsumer.Tests.Unitary.Base;

public class IdempotentConsumerTest
{
    private const string InstanceId = "Instance-ID";
    private const string ConstIdempotencyKey = "Test-Key-5678";

    private readonly IdempotentConsumer<IdempotentConsumerKeyMessageTest> _idempotentConsumer;
    private readonly Mock<IMembershipStrategy<IdempotentConsumerKeyMessageTest>> _memberShipStrategyMock;

    public IdempotentConsumerTest()
    {
        _memberShipStrategyMock = new Mock<IMembershipStrategy<IdempotentConsumerKeyMessageTest>>();
        _idempotentConsumer = new IdempotentConsumerImplementationTest(_memberShipStrategyMock.Object);
    }

    [Fact(DisplayName = "Deve retornar Status de ignorado quando já existir chave processo sem retorno")]
    public async Task ShouldReturnIgnoredStatusWhenExistKeyWithoutReturnProcess()
    {
        // arrange
        _memberShipStrategyMock.Setup(lnq => lnq.ExistsAsync(InstanceId, It.IsAny<IdempotentConsumerKeyMessageTest>())).Returns(Task.FromResult(true));

        // act
        var result = await _idempotentConsumer.ProcessAsync(InstanceId, new IdempotentConsumerKeyMessageTest(ConstIdempotencyKey), () => Task.CompletedTask);

        // assert
        result.Should().Be(CompletionStatus.Ignored);
        _memberShipStrategyMock.Verify(lnq => lnq.ExistsAsync(InstanceId, It.Is<IdempotentConsumerKeyMessageTest>(t => t.IdempotencyKey == ConstIdempotencyKey)), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar Status de ignorado quando já existir chave processo com retorno")]
    public async Task ShouldReturnIgnoredStatusWhenExistKeyWithReturnProcess()
    {
        // arrange
        _memberShipStrategyMock.Setup(lnq => lnq.ExistsAsync(InstanceId, It.IsAny<IdempotentConsumerKeyMessageTest>())).Returns(Task.FromResult(true));

        // act
        var result = await _idempotentConsumer.ProcessAsync(InstanceId, new IdempotentConsumerKeyMessageTest(ConstIdempotencyKey), () => Task.FromResult(new Result {Id = 22, Name = "SS"})!);

        // assert
        result.CompletionStatus.Should().Be(CompletionStatus.Ignored);
        result.Result!.Id.Should().Be(0);
        result.Result.Name.Should().BeNullOrEmpty();

        _memberShipStrategyMock.Verify(lnq => lnq.ExistsAsync(InstanceId, It.Is<IdempotentConsumerKeyMessageTest>(t => t.IdempotencyKey == ConstIdempotencyKey)), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar Status de consumido quando não existir chave processo sem retorno")]
    public async Task ShouldReturnConsumedStatusWhenNotFindKeyProcessWithoutReturn()
    {
        // arrange
        var memberShipStrategyMock = new Mock<IMembershipStrategyExtended<IdempotentConsumerKeyMessageTest>>();
        var idempotentConsumer = new IdempotentConsumerImplementationTest(memberShipStrategyMock.Object);

        var idempotentConsumerKeyMessageTest = new IdempotentConsumerKeyMessageTest(ConstIdempotencyKey);
        _memberShipStrategyMock.Setup(lnq => lnq.ExistsAsync(InstanceId, It.IsAny<IdempotentConsumerKeyMessageTest>())).Returns(Task.FromResult(false));

        // act
        var result = await idempotentConsumer.ProcessAsync(InstanceId, idempotentConsumerKeyMessageTest, () => Task.CompletedTask);

        // assert
        result.Should().Be(CompletionStatus.Consumed);

        memberShipStrategyMock.Verify(lnq => lnq.ExistsAsync(InstanceId, It.Is<IdempotentConsumerKeyMessageTest>(t => t.IdempotencyKey == ConstIdempotencyKey)), Times.Once);
        memberShipStrategyMock.Verify(lnq => lnq.SetStateAsync(InstanceId, idempotentConsumerKeyMessageTest, RepositoryEntryState.Processing), Times.Once);
        memberShipStrategyMock.Verify(lnq => lnq.AddAsync(InstanceId, idempotentConsumerKeyMessageTest), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar Status de consumido com estratégia extendida quando não existir chave processo sem retorno")]
    public async Task ShouldReturnConsumedStatusWithExtendedStrategyWhenNotFindKeyProcessWithoutReturn()
    {
        // arrange
        var idempotentConsumerKeyMessageTest = new IdempotentConsumerKeyMessageTest(ConstIdempotencyKey);
        _memberShipStrategyMock.Setup(lnq => lnq.ExistsAsync(InstanceId, It.IsAny<IdempotentConsumerKeyMessageTest>())).Returns(Task.FromResult(false));

        // act
        var result = await _idempotentConsumer.ProcessAsync(InstanceId, idempotentConsumerKeyMessageTest, () => Task.CompletedTask);

        // assert
        result.Should().Be(CompletionStatus.Consumed);

        _memberShipStrategyMock.Verify(lnq => lnq.ExistsAsync(InstanceId, It.Is<IdempotentConsumerKeyMessageTest>(t => t.IdempotencyKey == ConstIdempotencyKey)), Times.Once);
        _memberShipStrategyMock.Verify(lnq => lnq.AddAsync(InstanceId, idempotentConsumerKeyMessageTest), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar Status de consumido quando não existir chave processo com retorno")]
    public async Task ShouldReturnConsumedStatusWhenNotFindKeyProcessWithReturn()
    {
        // arrange
        var idempotentConsumerKeyMessageTest = new IdempotentConsumerKeyMessageTest(ConstIdempotencyKey);
        _memberShipStrategyMock.Setup(lnq => lnq.ExistsAsync(InstanceId, It.IsAny<IdempotentConsumerKeyMessageTest>())).Returns(Task.FromResult(false));

        // act
        var result = await _idempotentConsumer.ProcessAsync(InstanceId, idempotentConsumerKeyMessageTest, () => Task.FromResult(new Result {Id = 22, Name = "SS"})!);

        // assert
        result.CompletionStatus.Should().Be(CompletionStatus.Consumed);
        result.Result!.Id.Should().Be(22);
        result.Result.Name.Should().BeEquivalentTo("SS");

        _memberShipStrategyMock.Verify(lnq => lnq.ExistsAsync(InstanceId, It.Is<IdempotentConsumerKeyMessageTest>(t => t.IdempotencyKey == ConstIdempotencyKey)), Times.Once);
        _memberShipStrategyMock.Verify(lnq => lnq.AddAsync(InstanceId, idempotentConsumerKeyMessageTest), Times.Once);
    }

    [Fact(DisplayName = "Deve retornar Status de consumido com estratégia extendida quando não existir chave processo com retorno")]
    public async Task ShouldReturnConsumedStatusWithExtendedStrategyWhenNotFindKeyProcessWithtReturn()
    {
        var idempotentConsumerKeyMessageTest = new IdempotentConsumerKeyMessageTest(ConstIdempotencyKey);
        var memberShipStrategyMock = new Mock<IMembershipStrategyExtended<IdempotentConsumerKeyMessageTest>>();
        var idempotentConsumer = new IdempotentConsumerImplementationTest(memberShipStrategyMock.Object);

        _memberShipStrategyMock.Setup(lnq => lnq.ExistsAsync(InstanceId, It.IsAny<IdempotentConsumerKeyMessageTest>())).Returns(Task.FromResult(false));

        // act
        var result = await idempotentConsumer.ProcessAsync(InstanceId, idempotentConsumerKeyMessageTest, () => Task.FromResult(new Result {Id = 22, Name = "SS"})!);

        // assert
        result.CompletionStatus.Should().Be(CompletionStatus.Consumed);
        result.Result!.Id.Should().Be(22);
        result.Result.Name.Should().BeEquivalentTo("SS");

        memberShipStrategyMock.Verify(lnq => lnq.ExistsAsync(InstanceId, It.Is<IdempotentConsumerKeyMessageTest>(t => t.IdempotencyKey == ConstIdempotencyKey)), Times.Once);
        memberShipStrategyMock.Verify(lnq => lnq.SetStateAsync(InstanceId, idempotentConsumerKeyMessageTest, RepositoryEntryState.Processing), Times.Once);
        memberShipStrategyMock.Verify(lnq => lnq.AddAsync(InstanceId, idempotentConsumerKeyMessageTest), Times.Once);
    }

    [Fact(DisplayName = "Deve lançar exceção quando falhar ao processar processo sem retorno")]
    public async Task ShouldThrownExceptionWheFailedToProcessWithoutReturn()
    {
        // arrange
        _memberShipStrategyMock.Setup(lnq => lnq.ExistsAsync(InstanceId, It.IsAny<IdempotentConsumerKeyMessageTest>())).Returns(Task.FromResult(false));

        // act
        await _idempotentConsumer.Invoking(lnq => lnq.ProcessAsync(InstanceId, new IdempotentConsumerKeyMessageTest(ConstIdempotencyKey), () => throw new Exception("Fail")))
           .Should()
           .ThrowAsync<Exception>();

        // assert
        _memberShipStrategyMock.Verify(lnq => lnq.ExistsAsync(InstanceId, It.Is<IdempotentConsumerKeyMessageTest>(t => t.IdempotencyKey == ConstIdempotencyKey)), Times.Once);
    }

    [Fact(DisplayName = "Deve lançar exceção quando falhar ao processar processo com retorno")]
    public async Task ShouldThrownExceptionWheFailedToProcessWithReturn()
    {
        // arrange
        _memberShipStrategyMock.Setup(lnq => lnq.ExistsAsync(InstanceId, It.IsAny<IdempotentConsumerKeyMessageTest>())).Returns(Task.FromResult(false));

        // act
        await _idempotentConsumer.Invoking(lnq => lnq.ProcessAsync<Result>(InstanceId, new IdempotentConsumerKeyMessageTest(ConstIdempotencyKey), () => throw new Exception("Fail")))
           .Should()
           .ThrowAsync<Exception>();

        // assert
        _memberShipStrategyMock.Verify(lnq => lnq.ExistsAsync(InstanceId, It.Is<IdempotentConsumerKeyMessageTest>(t => t.IdempotencyKey == ConstIdempotencyKey)), Times.Once);
    }

    [Fact(DisplayName = "Deve lançar exceção com estratégia extendida quando falhar ao processar processo sem retorno")]
    public async Task ShouldThrownExceptionWithExtendedStrategyWheFailedToProcessWithoutReturn()
    {
        // arrange
        var idempotentConsumerKeyMessageTest = new IdempotentConsumerKeyMessageTest(ConstIdempotencyKey);
        var memberShipStrategyMock = new Mock<IMembershipStrategyExtended<IdempotentConsumerKeyMessageTest>>();
        var idempotentConsumer = new IdempotentConsumerImplementationTest(memberShipStrategyMock.Object);

        memberShipStrategyMock.Setup(lnq => lnq.ExistsAsync(InstanceId, It.IsAny<IdempotentConsumerKeyMessageTest>())).Returns(Task.FromResult(false));

        // act
        await idempotentConsumer.Invoking(lnq => lnq.ProcessAsync(InstanceId, idempotentConsumerKeyMessageTest, () => throw new Exception("Fail")))
           .Should()
           .ThrowAsync<Exception>();

        // assert
        memberShipStrategyMock.Verify(lnq => lnq.ExistsAsync(InstanceId, It.Is<IdempotentConsumerKeyMessageTest>(t => t.IdempotencyKey == ConstIdempotencyKey)), Times.Once);
        memberShipStrategyMock.Verify(lnq => lnq.SetStateAsync(InstanceId, idempotentConsumerKeyMessageTest, RepositoryEntryState.Processing), Times.Once);
        memberShipStrategyMock.Verify(lnq => lnq.RemoveAsync(InstanceId, idempotentConsumerKeyMessageTest), Times.Once);
    }

    [Fact(DisplayName = "Deve lançar exceção com estratégia extendida quando falhar ao processar processo com retorno")]
    public async Task ShouldThrownExceptionWithExtendedStrategyWheFailedToProcessWithReturn()
    {
        // arrange
        var idempotentConsumerKeyMessageTest = new IdempotentConsumerKeyMessageTest(ConstIdempotencyKey);
        var memberShipStrategyMock = new Mock<IMembershipStrategyExtended<IdempotentConsumerKeyMessageTest>>();
        var idempotentConsumer = new IdempotentConsumerImplementationTest(memberShipStrategyMock.Object);

        memberShipStrategyMock.Setup(lnq => lnq.ExistsAsync(InstanceId, It.IsAny<IdempotentConsumerKeyMessageTest>())).Returns(Task.FromResult(false));

        // act
        await idempotentConsumer.Invoking(lnq => lnq.ProcessAsync<Result>(InstanceId, idempotentConsumerKeyMessageTest, () => throw new Exception("Fail")))
           .Should()
           .ThrowAsync<Exception>();

        // assert
        memberShipStrategyMock.Verify(lnq => lnq.ExistsAsync(InstanceId, It.Is<IdempotentConsumerKeyMessageTest>(t => t.IdempotencyKey == ConstIdempotencyKey)), Times.Once);
        memberShipStrategyMock.Verify(lnq => lnq.SetStateAsync(InstanceId, idempotentConsumerKeyMessageTest, RepositoryEntryState.Processing), Times.Once);
        memberShipStrategyMock.Verify(lnq => lnq.RemoveAsync(InstanceId, idempotentConsumerKeyMessageTest), Times.Once);
    }

    public class IdempotentConsumerImplementationTest : IdempotentConsumer<IdempotentConsumerKeyMessageTest>
    {
        public IdempotentConsumerImplementationTest(IMembershipStrategy<IdempotentConsumerKeyMessageTest> membershipStrategy)
            : base(membershipStrategy)
        {
        }
    }

    public class IdempotentConsumerKeyMessageTest : IIdempotentConsumerKey
    {
        public string IdempotencyKey { get; }

        public IdempotentConsumerKeyMessageTest(string idempotencyKey)
        {
            IdempotencyKey = idempotencyKey;
        }
    }

    public class Result
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}