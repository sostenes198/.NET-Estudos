using Flurl.Http;
using Flurl.Http.Configuration;
using Moq;

namespace Scheduled.Message.Tests.Base.Mocks;

public static class FlurlRequestMock
{
    public static Mock<IFlurlRequest> Mock(Mock<IFlurlClient> flurlClientMock)
    {
        var flurlRequestMock = new Mock<IFlurlRequest>(MockBehavior.Strict);
        flurlRequestMock.Setup(lnq => lnq.EnsureClient()).Returns(flurlClientMock.Object);
        flurlRequestMock.Setup(lnq => lnq.Settings).Returns(new FlurlHttpSettings());
        flurlRequestMock.Setup(lnq => lnq.SendAsync(
                It.IsAny<HttpMethod>(), It.IsAny<HttpContent>(), It.IsAny<HttpCompletionOption>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Mock<IFlurlResponse>().Object);

        return flurlRequestMock;
    }
}