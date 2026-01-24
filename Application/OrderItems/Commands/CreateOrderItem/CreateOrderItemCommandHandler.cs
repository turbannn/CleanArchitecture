using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.CreateOrderItem
{
    public class CreateOrderItemCommandHandler
    {
        private readonly IOrderItemsRepository _orderItemRepository;

        public CreateOrderItemCommandHandler(IOrderItemsRepository orderItemRepository)
        {
            _orderItemRepository = orderItemRepository;
        }

        public async Task Handle(CreateOrderItemCommand request)
        {
            //Test validation
            if (request.Quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            var orderItem = new OrderItem()
            {
                Id = new Guid(),
                Quantity = request.Quantity,
                ProductName = request.ProductName,
                UnitPrice = request.UnitPrice
            };

            await _orderItemRepository.AddAsync(orderItem);
        }
    }
}
