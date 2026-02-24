using Application.OrderItems.Dto;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.CreateOrder
{
    public sealed record CreateOrderCommand(List<CreateOrderItemDto> OrderItems) : IRequest;
}
