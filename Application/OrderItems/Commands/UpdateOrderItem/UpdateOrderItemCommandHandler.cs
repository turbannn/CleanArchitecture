using Application.OrderItems.Commands.DeleteOrderItem;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.UpdateOrderItem
{
    public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand>
    {
        private readonly IOrderItemsRepository _orderItemRepository;
        private readonly IValidator<UpdateOrderItemCommand> _validator;

        public UpdateOrderItemCommandHandler(IOrderItemsRepository orderItemRepository, IValidator<UpdateOrderItemCommand> validator)
        {
            _orderItemRepository = orderItemRepository;
            _validator = validator;
        }

        public async Task Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
        {
            var res = await _validator.ValidateAsync(request, cancellationToken);

            if (!res.IsValid)
            {
                Console.WriteLine(res.Errors.First());
                return;
            }

            var oi = new OrderItem
            {
                Id = request.Id,
                ProductName = request.ProductName,
                Quantity = request.Quantity,
                UnitPrice = request.UnitPrice,
                StockKeepingUnit = request.StockKeepingUnit
            };

            await _orderItemRepository.UpdateAsync(oi, cancellationToken);
        }
    }
}
