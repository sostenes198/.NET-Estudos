using DocumentManager.Core.Models;
using DocumentManager.WorkFlows.Activities;
using Elsa.Scripting.Liquid.Messages;
using Fluid;
using MediatR;

namespace DocumentManager.WorkFlows.Scripting.Liquid;

public class ConfigureLiquidEngine : INotificationHandler<EvaluatingLiquidExpression>
{
    public Task Handle(EvaluatingLiquidExpression notification, CancellationToken cancellationToken)
    {
        var memberAccessStrategy = notification.TemplateContext.Options.MemberAccessStrategy;
            
        memberAccessStrategy.Register<Document>();
        memberAccessStrategy.Register<DocumentFile>();
            
        return Task.CompletedTask;
    }
}