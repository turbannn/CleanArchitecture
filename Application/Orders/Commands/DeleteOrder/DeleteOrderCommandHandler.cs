using Application.Orders.Commands.UpdateOrder;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrdersRepository _orderRepository;
    private readonly IValidator<DeleteOrderCommand> _validator;

    public DeleteOrderCommandHandler(IOrdersRepository orderRepository, IValidator<DeleteOrderCommand> validator)
    {
        _orderRepository = orderRepository;
        _validator = validator;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return;
        }

        await _orderRepository.DeleteAsync(request.Id, cancellationToken);
    }
}