using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FluentValidation;
using System.Linq;
using System.Collections.Generic;

// specify the reference explicitly
using ValidationException = Ordering.Application.Exceptions.ValidationException;

namespace Ordering.Application.Behaviors
{

    // define the pipieine to decide which type of request need to do the validation
    // and once pass, send to which handler that return TResponse type for the later processing
    public class ValidationBehaviour<TRequest,TResponse> : IPipelineBehavior<TRequest,TResponse>
        where TRequest: IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators ?? throw new ArgumentNullException(nameof(validators));   

        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {

                // define which type of request need to perform validatoin
                var context = new ValidationContext<TRequest>(request);


                // validation resuult
                var validationResults = await Task.WhenAll(_validators.Select(c => c.ValidateAsync(request, cancellationToken)));


                var failures = validationResults.SelectMany(f => f.Errors).Where(f => f != null).ToList();

                if(failures.Count != 0)
                    throw new ValidationException(failures);

         
            }

            // pass to the Request Handler that return TReponse type's request handler.
            return await next();

        }

    }
}
