using Application.OrderItems.Dto;
using Application.Orders.Commands.UpdateOrder;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, GlovoResult>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IUsersRepository _usersRepository;
    private readonly IValidator<CreateOrderCommand> _validator;
    private readonly IMapper<CreateOrderCommand, Order> _mapper;
    private readonly IMapper<CreateOrderItemDto, OrderItem> _orderItemMapper;

    public CreateOrderCommandHandler(IOrdersRepository ordersRepository,
        IUsersRepository usersRepository,
        IValidator<CreateOrderCommand> validator, 
        IMapper<CreateOrderCommand, Order> mapper, 
        IMapper<CreateOrderItemDto, OrderItem> orderItemDtoMapper)
    {
        _ordersRepository = ordersRepository;
        _usersRepository = usersRepository;
        _validator = validator;
        _mapper = mapper;
        _orderItemMapper = orderItemDtoMapper;
    }

    public async Task<GlovoResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return GlovoResult.Fail(res.Errors.First().ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var us = await _usersRepository.GetByIdAsync(request.UserId, cancellationToken);

        if(us is null)
            GlovoResult.Fail($"User with id {request.UserId} not found.", GlovoStatusCodes.NotFound);

        var orderItems = new List<OrderItem>();

        foreach (var oi in request.OrderItems)
        {
            var item = _orderItemMapper.Map(oi);
            if(item is not null)
            {
                item.Id = Guid.NewGuid();
                orderItems.Add(item);
            }
        }

        var order = _mapper.Map(request);
        order.Id = Guid.NewGuid();
        order.OrderDate = DateTime.UtcNow;
        order.Items = orderItems;

        var createRes = await _ordersRepository.AddAsync(order, cancellationToken);

        if (!createRes)
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        else
            return GlovoResult.Success();
    }
}
