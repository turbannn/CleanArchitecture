using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
{
    private readonly IOrdersRepository _orderRepository;

    public DeleteOrderCommandHandler(IOrdersRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        if (request.Id.Equals(Guid.Empty))
            throw new ArgumentException("Id can't be 0");

        await _orderRepository.DeleteAsync(request.Id, cancellationToken);
    }
}