using Domain.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.DeleteOrderItem;

public sealed record DeleteOrderItemCommand(Guid Id) : IRequest<GlovoResult>;
