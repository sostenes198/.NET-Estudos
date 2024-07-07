using Microsoft.AspNetCore.Mvc;
using Scheduled.Message.Api.Controllers.V1.Base;
using Scheduled.Message.Api.Models;
using Scheduled.Message.Api.Presenters.Http.ScheduleVollProcess;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.Exentions;
using Scheduled.Message.Application.UseCases.ScheduleVollProcess;

namespace Scheduled.Message.Api.Controllers.V1;

[Route(BasePath + "/voll")]
public class VollScheduleController(
    ILogger<VollScheduleController> logger,
    IUseCaseManager manager,
    IScheduleVollProcessUseCaseOutput output)
    : V1Controller
{ 
    [HttpPost("schedule")]
    public async Task<IActionResult> ScheduleAsync([FromBody] ScheduleVollProcessModel model, CancellationToken token)
    {
        using (logger.BeginNamedScope("controller-voll-schedule",
                   ("JobName", model.JobName),
                   ("TopicName", model.TopicName)))
        {
            logger.LogInformation("Initialize UseCase ScheduleVollProcess with input {Input}", model);

            await manager.ExecuteAsync(
                new ScheduleVollProcessUseCaseInput(
                    model.When,
                    model.JobName,
                    model.TopicName,
                    model.Key.ToString(),
                    model.Value.ToString(),
                    model.Headers.ToString()
                ),
                output,
                token);

            logger.LogInformation("End UseCase ScheduleVollProcess");

             return ((ScheduleVollProcessPresenter)output).Result();
        }
    }
}