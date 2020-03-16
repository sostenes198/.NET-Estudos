using EstudosMediator.Commands.Customer;
using FluentValidation;

namespace EstudosMediator.Validators.Customers
{
    public class CustomerUpdateCommandValidator : AbstractValidator<CustomerUpdateCommand>
    {
        public CustomerUpdateCommandValidator()
        {
            RuleFor(lnq => lnq.Id)
                .NotEmpty();

            RuleFor(lnq => lnq.FirstName)
                .NotEmpty();
            
            RuleFor(lnq => lnq.LastName)
                .NotEmpty();

            RuleFor(lnq => lnq.Phone)
                .NotEmpty();
            
            RuleFor(lnq => lnq.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}