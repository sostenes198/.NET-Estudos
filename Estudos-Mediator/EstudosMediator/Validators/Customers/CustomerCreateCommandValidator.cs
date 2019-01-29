using EstudosMediator.Commands.Customer;
using FluentValidation;

namespace EstudosMediator.Validators.Customers
{
    public class CustomerCreateCommandValidator : AbstractValidator<CustomerCreateCommand>
    {
        public CustomerCreateCommandValidator()
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