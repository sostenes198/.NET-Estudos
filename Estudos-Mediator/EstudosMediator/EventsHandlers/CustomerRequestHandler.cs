using System.Threading;
using System.Threading.Tasks;
using EstudosMediator.Commands.Customer;
using EstudosMediator.Core;
using EstudosMediator.Entity.Customer;
using EstudosMediator.Infra;
using EstudosMediator.Notifications;
using EstudosMediator.Validators;
using MediatR;

namespace EstudosMediator.EventsHandlers
{
    public class CustomerRequestHandler :
        IRequestHandler<CustomerCreateCommand, string>,
        IRequestHandler<CustomerUpdateCommand, string>,
        IRequestHandler<CustomerDeleteCommand, string>
    {
        private readonly IMediator _mediator;
        private readonly ICustomerRepository _customerRepository;
        private readonly NotificationContext _context;

        public CustomerRequestHandler(IMediator mediator, ICustomerRepository customerRepository, NotificationContext context)
        {
            _mediator = mediator;
            _customerRepository = customerRepository;
            _context = context;
        }
        
        public async Task<string> Handle(CustomerCreateCommand request, CancellationToken cancellationToken)
        {
            var customer = new CustomerEntity(request.Id, request.FirstName,request.LastName, request.Email, request.Phone);
            await _customerRepository.Save(customer);
            await _mediator.Publish(new CustomerActionNotification
            {
                Action = ActionNotification.Criado,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            }, cancellationToken);
            
            

            return "Cliente registrado com sucesso";
        }

        public async Task<string> Handle(CustomerUpdateCommand request, CancellationToken cancellationToken)
        {
            var customer = new CustomerEntity(request.Id, request.FirstName,request.LastName, request.Email, request.Phone);
            await _customerRepository.Update(request.Id, customer);
            await _mediator.Publish(new CustomerActionNotification
            {
                Action = ActionNotification.Atualizado,
                Email = customer.Email,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            }, cancellationToken);

            return "Cliente atualizado com sucesso";
        }

        public async Task<string> Handle(CustomerDeleteCommand request, CancellationToken cancellationToken)
        {
            var client = await _customerRepository.GetById(request.Id);
            await _customerRepository.Delete(request.Id);

            await _mediator.Publish(new CustomerActionNotification
            {
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                Action = ActionNotification.Excluido
            }, cancellationToken);

            return "Cliente excluido com sucesso";
        }
    }
}