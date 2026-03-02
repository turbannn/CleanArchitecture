using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.DeleteOrder
{
    public sealed record DeleteOrderCommand(Guid Id) : IRequest;
}
