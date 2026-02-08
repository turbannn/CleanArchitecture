using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.CreateOrderItem;

public sealed record CreateOrderItemCommand(decimal UnitPrice, string ProductName, int Quantity) : IRequest;
