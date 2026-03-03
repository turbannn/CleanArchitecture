using Application.OrderItems.Commands.CreateOrderItem;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.DeleteOrderItem;

public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand>
{
    private readonly IOrderItemsRepository _orderItemsRepository;
    private readonly IValidator<DeleteOrderItemCommand> _validator;

    public DeleteOrderItemCommandHandler(IOrderItemsRepository orderItemsRepository, IValidator<DeleteOrderItemCommand> validator)
    {
        _orderItemsRepository = orderItemsRepository;
        _validator = validator;
    }

    public async Task Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return;
        }

        await _orderItemsRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
