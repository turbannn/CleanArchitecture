using Domain.Entities;
using Domain.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Queries.GetOrderItemById;

public sealed record GetOrderItemByIdQuery(Guid Id) : IRequest<GenericGlovoResult<OrderItem>>;
