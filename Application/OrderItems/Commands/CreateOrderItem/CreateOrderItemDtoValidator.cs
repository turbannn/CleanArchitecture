using Application.OrderItems.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.CreateOrderItem;

internal class CreateOrderItemDtoValidator : AbstractValidator<CreateOrderItemDto>
{
    public CreateOrderItemDtoValidator()
    {
        //Int
        RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("UnitPrice must be greater than zero.");
        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        //String
        RuleFor(x => x.ProductName)
            .NotEmpty().WithMessage("ProductName is required.")
            .MaximumLength(40).WithMessage("ProductName must be maximum 40 symbols");

        RuleFor(x => x.StockKeepingUnit)
            .NotEmpty().WithMessage("StockKeepingUnit is required.")
            .MaximumLength(40).WithMessage("StockKeepingUnit must be maximum 40 symbols");
    }
}
