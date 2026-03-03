using Application.OrderItems.Commands.UpdateOrderItem;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Queries.GetOrderItemById;

public class GetOrderItemByIdQueryHandler : IRequestHandler<GetOrderItemByIdQuery, OrderItem>
{
    private readonly IOrderItemsRepository _orderItemRepository;
    private readonly IValidator<GetOrderItemByIdQuery> _validator;

    public GetOrderItemByIdQueryHandler(IOrderItemsRepository orderItemRepository, IValidator<GetOrderItemByIdQuery> validator)
    {
        _orderItemRepository = orderItemRepository;
        _validator = validator;
    }

    public async Task<OrderItem> Handle(GetOrderItemByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return new OrderItem { ProductName = "null" };
        }

        var orderItem = await _orderItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (orderItem is null)
        {
            throw new KeyNotFoundException($"OrderItem with Id {request.Id} was not found.");
        }

        return orderItem;
    }
}