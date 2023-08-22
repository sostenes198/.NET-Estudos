using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EstudosMediator.Validators;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace EstudosMediator.PipelineBehavior
{
    public class ValidatorRequestBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly NotificationContext _notificationContext;
        private readonly IEnumerable<IValidator> _validators;

        public ValidatorRequestBehavior(NotificationContext notificationContext, IEnumerable<IValidator<TRequest>> validators)
        {
            _notificationContext = notificationContext;
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var tsk = _validators
                .Select(async lnq => await lnq.ValidateAsync(request, cancellationToken));

            var failures = await Task.WhenAll(tsk);

            var result = failures.SelectMany(lnq => lnq.Errors)
                .Where(lnq => lnq != default)
                .ToList();
            
            return await (result.Any()
                ? Errors(result)
                : next());
        }

        private Task<TResponse> Errors(IList<ValidationFailure> failures)
        {
            _notificationContext.AddNotifications(failures);
            return Task.FromResult<TResponse>(default);
        }
    }
}