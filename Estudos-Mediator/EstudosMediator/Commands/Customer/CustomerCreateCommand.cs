using MediatR;

namespace EstudosMediator.Commands.Customer
{
    public class CustomerCreateCommand : IRequest<string>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}