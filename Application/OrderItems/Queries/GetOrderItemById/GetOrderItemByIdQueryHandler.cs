using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Queries.GetOrderItemById;

public class GetOrderItemByIdQueryHandler : IRequestHandler<GetOrderItemByIdQuery, GenericGlovoResult<OrderItem>>
{
    private readonly IOrderItemsRepository _orderItemRepository;
    private readonly IValidator<GetOrderItemByIdQuery> _validator;

    public GetOrderItemByIdQueryHandler(IOrderItemsRepository orderItemRepository, IValidator<GetOrderItemByIdQuery> validator)
    {
        _orderItemRepository = orderItemRepository;
        _validator = validator;
    }

    public async Task<GenericGlovoResult<OrderItem>> Handle(GetOrderItemByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return GenericGlovoResult<OrderItem>.Fail(res.Errors.First().ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var orderItem = await _orderItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (orderItem is null)
        {
            return GenericGlovoResult<OrderItem>.Fail("Order item was not found", GlovoStatusCodes.NotFound);
        }

        return GenericGlovoResult<OrderItem>.Success(orderItem);
    }
}