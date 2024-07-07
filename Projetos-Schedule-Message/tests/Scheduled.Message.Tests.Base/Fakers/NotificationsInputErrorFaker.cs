using Scheduled.Message.Application.Boundaries.UseCases.Validators;

namespace Scheduled.Message.Tests.Base.Fakers;

public static class NotificationsInputErrorFaker
{
    public static NotificationsInputError Create()
    {
        var errors = new NotificationsInputError();
        errors.Add("PROP_1", "ERROR_1");
        errors.Add("PROP_1", "ERROR_2");
        errors.Add("PROP_2", "ERROR_1");

        return errors;
    }
}