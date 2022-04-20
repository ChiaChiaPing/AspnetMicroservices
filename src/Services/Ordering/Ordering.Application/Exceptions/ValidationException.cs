using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace Ordering.Application.Exceptions
{
    public class ValidationException: ApplicationException
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException()
            :base("One or more error occurred during validation")
        {

            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            :this()
        {

            Errors = failures
                        .GroupBy(f => f.PropertyName, f => f.ErrorMessage)
                        .ToDictionary(fg => fg.Key, fg => fg.ToArray());

        }
     
    }
}
