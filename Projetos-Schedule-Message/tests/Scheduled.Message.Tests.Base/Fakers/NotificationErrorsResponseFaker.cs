using Scheduled.Message.Api.Presenters.Base;

namespace Scheduled.Message.Tests.Base.Fakers;

public static class NotificationErrorsResponseFaker
{
    public static NotificationErrorsResponse Create()
    {
        return new NotificationErrorsResponse(new Dictionary<string, string[]>
        {
            { "PROP_1", ["ERROR_1", "ERROR_2"] },
            { "PROP_2", ["ERROR_1"] }
        });
    }
}