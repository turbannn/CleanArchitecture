using Application.Orders.Queries.GetOrderById;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IValidator<UpdateOrderCommand> _validator;

    public UpdateOrderCommandHandler(IOrdersRepository ordersRepository, IValidator<UpdateOrderCommand> validator)
    {
        _ordersRepository = ordersRepository;
        _validator = validator;
    }

    public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return;
        }

        var order = new Order
        {
            Id = request.Id,
            ShippingAddress = request.ShippingAddress,
            Notes = request.Notes
        };

        await _ordersRepository.UpdateAsync(order, cancellationToken);
    }
}
