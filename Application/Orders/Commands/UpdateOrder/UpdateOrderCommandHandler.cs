using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IOrdersRepository _ordersRepository;

        public UpdateOrderCommandHandler(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.ShippingAddress))
                throw new ArgumentException("Shipping address cannot be null or empty.");

            if (string.IsNullOrEmpty(request.Notes))
                throw new ArgumentException("Notes cannot be null or empty");

            var order = new Order
            {
                Id = request.Id,
                ShippingAddress = request.ShippingAddress,
                Notes = request.Notes
            };

            await _ordersRepository.UpdateAsync(order, cancellationToken);
        }
    }
}
