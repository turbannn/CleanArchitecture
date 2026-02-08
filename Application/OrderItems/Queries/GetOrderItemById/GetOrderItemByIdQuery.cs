using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Queries.GetOrderItemById
{
    public sealed record GetOrderItemByIdQuery(Guid Id) : IRequest<OrderItem>;
}
