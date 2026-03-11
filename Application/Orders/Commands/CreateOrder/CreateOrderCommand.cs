using Application.OrderItems.Dto;
using Domain.Entities;
using Domain.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.CreateOrder;

public sealed record CreateOrderCommand(string ShippingAddress, string Notes, Guid UserId, List<CreateOrderItemDto> OrderItems) : IRequest<GlovoResult>;
