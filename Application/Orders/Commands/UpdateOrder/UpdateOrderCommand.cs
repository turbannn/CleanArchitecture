using Domain.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.UpdateOrder;

public sealed record UpdateOrderCommand(Guid Id, string ShippingAddress, string Notes) : IRequest<GlovoResult>;
