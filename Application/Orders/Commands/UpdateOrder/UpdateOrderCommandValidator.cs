using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            //Ids
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Order Id is required.");

            //Strings
            RuleFor(x => x.ShippingAddress)
                .NotEmpty().WithMessage("ShippingAddress is required.")
                .MaximumLength(50).WithMessage("ShippingAddress cannot exceed 50 characters.");

            RuleFor(x => x.Notes)
                .NotEmpty().WithMessage("Notes is required.")
                .MaximumLength(150).WithMessage("Notes cannot exceed 150 characters.");
        }
    }
}
