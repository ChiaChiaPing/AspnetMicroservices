using System;
using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.CheckOutOrder
{

    // 針對什麼樣的request 在送到handler 之前建立preprocessor, 並對其request 做驗證
    // can install fluentValidation and inheriit AbstractValidator
    public class CheckOutOrderCommandValidator: AbstractValidator<CheckOutOrderCommand>
    {
        public CheckOutOrderCommandValidator()
        {

            // witth message, if fail on the validation then show the message
            // Rule For => to validate the property of requeust
            RuleFor(c => c.UserName)
                .NotEmpty().WithMessage("{UserName} should not be empty")
                .NotNull()
                .MaximumLength(50).WithMessage("{UserName} should not be exceed 50 characters");

            RuleFor(c => c.EmailAddress)
                 .NotEmpty().WithMessage("{EmailAddress} should not be empty");
                 

            RuleFor(c => c.TotalPrice)
                .NotEmpty().WithMessage("{TotalPrice} should not be empty")
                .GreaterThan(0).WithMessage("{TotalPrice} should be greater than 0");



            

        }
    }
}
