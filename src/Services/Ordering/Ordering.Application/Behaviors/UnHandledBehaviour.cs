using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ordering.Application.Behaviors
{

    // this is a behavior that do nothing for Validator.
    public class UnHandledBehaviour<TRequest,TReponse> : IPipelineBehavior<TRequest,TReponse>
        where TRequest: IRequest<TReponse>
    {

        private readonly ILogger<TRequest> _logger;
       
        public UnHandledBehaviour(ILogger<TRequest> logger)
        {
            _logger = logger;

        }

        public async Task<TReponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TReponse> next)
        {
            try
            {

                return await next();
            }
            catch (Exception ex)
            {
                var requestName = typeof(TRequest).Name;
                _logger.LogError(ex, "Application Request: Unhandled Exception for Request {Name} {@Request}", requestName, request);
                throw;
            }
        }
    }
}
