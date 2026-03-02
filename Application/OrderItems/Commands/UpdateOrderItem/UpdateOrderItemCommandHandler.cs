using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.UpdateOrderItem
{
    public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand>
    {
        private readonly IOrderItemsRepository _orderItemRepository;

        public UpdateOrderItemCommandHandler(IOrderItemsRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
        {
            if (request.UnitPrice <= 0)
                throw new InvalidDataException("Unit price can't be less or equal zero");

            if(request.Quantity <= 0)
                throw new InvalidDataException("Quantity can't be less or equal zero");
            
            var oi = new OrderItem
            {
                Id = request.Id,
                ProductName = request.ProductName,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice
            };

            await _orderItemRepository.UpdateAsync(oi, cancellationToken);
        }
    }
}
