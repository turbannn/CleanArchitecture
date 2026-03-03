using Application.OrderItems.Commands.UpdateOrderItem;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, Order>
    {
        private readonly IOrdersRepository _ordersRepository;
        private readonly IValidator<GetOrderByIdQuery> _validator;

        public GetOrderByIdQueryHandler(IOrdersRepository ordersRepository, IValidator<GetOrderByIdQuery> validator)
        {
            _ordersRepository = ordersRepository;
            _validator = validator;
        }

        public async Task<Order> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var res = await _validator.ValidateAsync(request, cancellationToken);

            if (!res.IsValid)
            {
                Console.WriteLine(res.Errors.First());
                return new Order { Notes = "null" };
            }

            var order = await _ordersRepository.GetByIdAsync(request.Id, cancellationToken);

            if (order is null)
                throw new NullReferenceException("Order not found");

            return order;
        }
    }
}
