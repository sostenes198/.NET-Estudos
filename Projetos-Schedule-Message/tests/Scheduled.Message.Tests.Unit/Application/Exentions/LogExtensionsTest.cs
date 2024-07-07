using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Scheduled.Message.Application.Exentions;
using Scheduled.Message.Tests.Base.Utils;

namespace Scheduled.Message.Tests.Unit.Application.Exentions;

public class LogExtensionsTest
{
    private readonly ValueTuple<string, object>[] _properties;
    private readonly IDictionary<string, object> _dictionaryProperties;
    private readonly Mock<ILogger> _loggerMock;

    public LogExtensionsTest()
    {
        _properties =
        [
            ("PROP_1", "VALUE_1"),
            ("PROP_2", "VALUE_2")
        ];
        _dictionaryProperties = _properties.ToDictionary();

        _loggerMock = new Mock<ILogger>();
    }

    [Fact]
    public void Should_Create_Begin_Named_Scope_Logger_Successfully()
    {
        // Arrange - Act
        _loggerMock.Object.BeginNamedScope("LogExtensionsTest", _properties);

        // Assert
        _loggerMock.Verify(lnq => lnq.BeginScope(It.Is<IDictionary<string, object>>(t => MatchBeginNamedScope(t))));
    }

    private bool MatchBeginNamedScope(IDictionary<string, object> properties)
    {
        properties.Last().Key.Should().Be("LogExtensionsTest.Scope");
        properties.Remove(properties.Last().Key);

        properties.Should().BeEquivalentTo(_dictionaryProperties);

        return true;
    }


    [Fact]
    public void Should_Create_Begin_Property_Scope_Logger_Successfully()
    {
        // Arrange - Act
        _loggerMock.Object.BeginPropertyScope(_properties);

        // Assert
        _loggerMock.Verify(lnq => lnq.BeginScope(MoqAssert.Assert(_dictionaryProperties)));
    }
}