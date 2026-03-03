using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.DeleteOrderItem;

public class DeleteOrderItemCommandValidator : AbstractValidator<DeleteOrderItemCommand>
{
    public DeleteOrderItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
