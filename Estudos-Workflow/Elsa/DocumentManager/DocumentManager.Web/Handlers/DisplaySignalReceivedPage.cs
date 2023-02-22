﻿using Elsa.Activities.Http.Events;
using MediatR;

namespace DocumentManager.Web.Handlers;

/// <summary>
/// Responds to the Accept or Reject signal when the user clicks either option from the email.
/// </summary>
public class DisplaySignalReceivedPage : INotificationHandler<HttpTriggeredSignal>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DisplaySignalReceivedPage(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Task Handle(HttpTriggeredSignal notification, CancellationToken cancellationToken)
    {
        var affectedWorkflows = notification.AffectedWorkflows;

        // Exit if no workflows were affected.
        if (affectedWorkflows.Count == 0)
            return Task.CompletedTask;

        var signalName = notification.SignalModel.Name;
        var response = _httpContextAccessor.HttpContext!.Response;

        // Redirect to the appropriate page.
        switch (signalName)
        {
            case "Approve":
                response.Redirect("/leave-request-approved");
                break;
            case "Reject":
                response.Redirect("/leave-request-denied");
                break;
        }

        return Task.CompletedTask;
    }
}