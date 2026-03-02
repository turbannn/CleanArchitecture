using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.DeleteOrderItem;

public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand>
{
    private readonly IOrderItemsRepository _orderItemsRepository;

    public DeleteOrderItemCommandHandler(IOrderItemsRepository orderItemsRepository)
    {
        _orderItemsRepository = orderItemsRepository;
    }

    public async Task Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
    {
        if (request.Id.Equals(Guid.Empty))
            throw new ArgumentException("Id can't be 0");

        await _orderItemsRepository.DeleteAsync(request.Id, cancellationToken);
    }
}
