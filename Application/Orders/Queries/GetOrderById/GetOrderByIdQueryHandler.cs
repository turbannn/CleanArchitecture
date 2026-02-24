using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
    {
        private readonly IOrdersRepository _ordersRepository;

        public GetOrderByIdQueryHandler(IOrdersRepository ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.Id.Equals(Guid.Empty))
                throw new NullReferenceException();

            var order = await _ordersRepository.GetByIdAsync(request.Id, cancellationToken);

            if (order is null)
                throw new NullReferenceException("Order not found");

            return order;
        }
    }
}
