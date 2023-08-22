using MediatR;

namespace EstudosMediator.Commands.Customer
{
    public class CustomerDeleteCommand : IRequest<string>
    {
        public int Id { get; set; }
    }
}