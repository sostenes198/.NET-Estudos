using FluentValidation;

namespace Scheduled.Message.Application.UseCases.TriggeredVollSchedule;

public class TriggeredVollScheduleUseCaseInputValidator : AbstractValidator<TriggeredVollScheduleUseCaseInput>
{
    public TriggeredVollScheduleUseCaseInputValidator()
    {
        RuleFor(lnq => lnq.When)
            .NotEmpty();

        RuleFor(lnq => lnq.JobName)
            .NotEmpty();

        RuleFor(lnq => lnq.TopicName)
            .NotEmpty();

        RuleFor(lnq => lnq.Key)
            .NotEmpty();
    }
}