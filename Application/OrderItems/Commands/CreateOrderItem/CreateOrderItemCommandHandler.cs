using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.CreateOrderItem
{
    public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand>
    {
        private readonly IOrderItemsRepository _orderItemRepository;
        private readonly IValidator<CreateOrderItemCommand> _validator;

        public CreateOrderItemCommandHandler(IOrderItemsRepository orderItemRepository, IValidator<CreateOrderItemCommand> validator)
        {
            _orderItemRepository = orderItemRepository;
            _validator = validator;
        }

        public async Task Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var res = await _validator.ValidateAsync(request, cancellationToken);

            if (!res.IsValid)
            {
                Console.WriteLine(res.Errors.First());
                return;
            }

            var oi = await _orderItemRepository.GetByIdAsync(request.OrderId, cancellationToken);

            if(oi is null)
                throw new NullReferenceException();
            
            var orderItem = new OrderItem()
            {
                Id = new Guid(),
                Quantity = request.Quantity,
                ProductName = request.ProductName,
                UnitPrice = request.UnitPrice,
                StockKeepingUnit = request.StockKeepingUnit,
                OrderId = request.OrderId
            };

            await _orderItemRepository.AddAsync(orderItem, cancellationToken);
        }
    }
}
