using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.UpdateOrderItem
{
    public sealed record UpdateOrderItemCommand(Guid Id, decimal UnitPrice, string ProductName, int Quantity, string StockKeepingUnit) : IRequest;
}
