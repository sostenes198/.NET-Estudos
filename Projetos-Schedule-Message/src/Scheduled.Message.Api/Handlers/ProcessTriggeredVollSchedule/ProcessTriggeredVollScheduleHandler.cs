using Scheduled.Message.Application.Boundaries.Scheduler.Handlers;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.Exentions;
using Scheduled.Message.Application.UseCases.TriggeredVollSchedule;
using Scheduled.Message.Domain.ScheduleVollProcess;

namespace Scheduled.Message.Api.Handlers.ProcessTriggeredVollSchedule;

public class ProcessTriggeredVollScheduleHandler(
    ILogger<ProcessTriggeredVollScheduleHandler> logger,
    IUseCaseManager useCaseManager,
    ITriggeredVollScheduleOutput output) : IProcessTriggeredVollScheduleHandler
{
    public async Task ExecuteAsync(VollProcessSchedule? input, CancellationToken token)
    {
        using (logger.BeginNamedScope("handler-troggered-voll-schedule",
                   ("JobName", input?.JobName ?? ""),
                   ("TopicName", input?.TopicName ?? "")))
        {
            logger.LogInformation("Initialize UseCase TriggeredVollSchedule with input {Input}", input);

            var formatedInput = input ?? VollProcessSchedule.Empty;

            var useCaseInput = new TriggeredVollScheduleUseCaseInput(
                formatedInput.When,
                formatedInput.JobName,
                formatedInput.TopicName,
                formatedInput.Key,
                formatedInput.Value,
                formatedInput.Headers
            );

            await useCaseManager.ExecuteAsync(useCaseInput, output, token);

            logger.LogInformation("End UseCase TriggeredVollSchedule");

            output.Success();
        }
    }
}