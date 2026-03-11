using Domain.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.CreateOrderItem;

public sealed record CreateOrderItemCommand(decimal UnitPrice, string ProductName, int Quantity, string StockKeepingUnit, Guid OrderId) : IRequest<GlovoResult>;
