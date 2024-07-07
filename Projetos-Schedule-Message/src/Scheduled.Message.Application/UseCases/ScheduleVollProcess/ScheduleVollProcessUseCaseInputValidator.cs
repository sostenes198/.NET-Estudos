using FluentValidation;

namespace Scheduled.Message.Application.UseCases.ScheduleVollProcess;

public class ScheduleVollProcessUseCaseInputValidator : AbstractValidator<ScheduleVollProcessUseCaseInput>
{
    public ScheduleVollProcessUseCaseInputValidator()
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