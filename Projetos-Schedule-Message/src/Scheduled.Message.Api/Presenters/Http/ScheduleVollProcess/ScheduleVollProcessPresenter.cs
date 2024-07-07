using Microsoft.AspNetCore.Mvc;
using Scheduled.Message.Api.Presenters.Http.Base;
using Scheduled.Message.Application.UseCases.ScheduleVollProcess;

namespace Scheduled.Message.Api.Presenters.Http.ScheduleVollProcess;

public sealed class ScheduleVollProcessPresenter : BaseHttpPresenter,
    IScheduleVollProcessUseCaseOutput
{
    public void Success()
    {
        Result = () => new OkResult();
    }
}