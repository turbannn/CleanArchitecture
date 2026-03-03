using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Queries.GetOrderItemById;

public class GetOrderItemByIdQueryValidator : AbstractValidator<GetOrderItemByIdQuery>
{
    public GetOrderItemByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");
    }
}
