using Application.OrderItems.Commands.CreateOrderItem;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;
    public CreateOrderCommandHandler(IOrdersRepository ordersRepository)
    {
        _ordersRepository = ordersRepository;
    }

    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if(request.OrderItems.Count == 0)
            throw new ArgumentException("Order must contain at least one item.");

        //Test
        var orderItems = new List<OrderItem>();

        foreach (var oi in request.OrderItems)
        {
            orderItems.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductName = oi.ProductName,
                UnitPrice = oi.UnitPrice,
                Quantity = oi.Quantity,
            });
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            Items = orderItems
        };

        await _ordersRepository.AddAsync(order, cancellationToken);
    }
}
