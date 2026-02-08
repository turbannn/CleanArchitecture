using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Queries.GetOrderItemById;

public class GetOrderItemByIdQueryHandler : IRequestHandler<GetOrderItemByIdQuery, OrderItem>
{
    private readonly IOrderItemsRepository _orderItemRepository;
    public GetOrderItemByIdQueryHandler(IOrderItemsRepository orderItemRepository)
    {
        _orderItemRepository = orderItemRepository; 
    }

    public async Task<OrderItem> Handle(GetOrderItemByIdQuery request, CancellationToken cancellationToken)
    {
        var orderItem = await _orderItemRepository.GetAsync(request.Id, cancellationToken);

        if (orderItem is null)
        {
            throw new KeyNotFoundException($"OrderItem with Id {request.Id} was not found.");
        }

        return orderItem;
    }
}