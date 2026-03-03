using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.UpdateOrderItem
{
    public class UpdateOrderItemCommandValidator : AbstractValidator<UpdateOrderItemCommand>
    {
        public UpdateOrderItemCommandValidator()
        {
            //Ids
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

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
}
