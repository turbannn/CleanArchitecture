using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Queries.GetOrderById
{
    public sealed record GetOrderByIdQuery(Guid Id) : IRequest<Order>;
}
