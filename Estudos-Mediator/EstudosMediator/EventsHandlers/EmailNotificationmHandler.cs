using System;
using System.Threading;
using System.Threading.Tasks;
using EstudosMediator.Notifications;
using MediatR;

namespace EstudosMediator.EventsHandlers
{
    public class EmailHandler : INotificationHandler<CustomerActionNotification>
    {
        public Task Handle(CustomerActionNotification notification, CancellationToken cancellationToken)
        {
            return Task.Run(() => Console.WriteLine("Email event: O cliente {0} {1} foi {2} com sucesso", notification.FirstName, notification.LastName, notification.Action.ToString().ToLower()), cancellationToken);
        }
    }
}

