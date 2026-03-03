using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Order Id must not be empty.");
        }
    }
}
