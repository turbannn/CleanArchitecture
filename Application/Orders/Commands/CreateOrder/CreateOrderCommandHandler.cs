using Application.OrderItems.Commands.CreateOrderItem;
using Application.OrderItems.Dto;
using Application.Orders.Commands.UpdateOrder;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IValidator<CreateOrderCommand> _validator;

    public CreateOrderCommandHandler(IOrdersRepository ordersRepository, IValidator<CreateOrderCommand> validator)
    {
        _ordersRepository = ordersRepository;
        _validator = validator;
    }

    public async Task Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return;
        }

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
                StockKeepingUnit = oi.StockKeepingUnit
            });
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            OrderDate = DateTime.UtcNow,
            ShippingAddress = request.ShippingAddress,
            Notes = request.Notes,
            Items = orderItems
        };

        await _ordersRepository.AddAsync(order, cancellationToken);
    }
}
