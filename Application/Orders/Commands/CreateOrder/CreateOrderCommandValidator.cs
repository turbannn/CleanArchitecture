using Application.OrderItems.Commands.CreateOrderItem;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            //Strings
            RuleFor(x => x.ShippingAddress)
                .NotEmpty().WithMessage("ShippingAddress is required.")
                .MaximumLength(50).WithMessage("ShippingAddress cannot exceed 50 characters.");

            RuleFor(x => x.Notes)
                .NotEmpty().WithMessage("Notes is required.")
                .MaximumLength(150).WithMessage("Notes cannot exceed 150 characters.");

            //ForEach
            RuleForEach(x => x.OrderItems)
                .SetValidator(new CreateOrderItemDtoValidator())
                .WithMessage("Each OrderItem must be valid.");
        }
    }
}
