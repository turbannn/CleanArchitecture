using Application.OrderItems.Dto;
using Application.Orders.Commands.UpdateOrder;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<CreateOrderCommandHandler> _logger;

    public CreateOrderCommandHandler(IOrdersRepository ordersRepository,
        IUsersRepository usersRepository,
        IValidator<CreateOrderCommand> validator, 
        IMapper<CreateOrderCommand, Order> mapper, 
        IMapper<CreateOrderItemDto, OrderItem> orderItemDtoMapper,
        ILogger<CreateOrderCommandHandler> logger)
    {
        _ordersRepository = ordersRepository;
        _usersRepository = usersRepository;
        _validator = validator;
        _mapper = mapper;
        _orderItemMapper = orderItemDtoMapper;
        _logger = logger;
    }

    public async Task<GlovoResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateOrderCommand for user {UserId}", request.UserId);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("CreateOrderCommand validation failed for user {UserId}: {Error}", request.UserId, firstError.ErrorMessage);
            return GlovoResult.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var us = await _usersRepository.GetByIdAsync(request.UserId, cancellationToken);

        if(us is null)
        {
            _logger.LogWarning("User {UserId} not found while creating order", request.UserId);
            return GlovoResult.Fail($"User with id {request.UserId} not found.", GlovoStatusCodes.NotFound);
        }

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
        {
            _logger.LogError("Failed to persist order {OrderId}", order.Id);
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        }

        _logger.LogInformation("Order {OrderId} created successfully", order.Id);
        return GlovoResult.Success();
    }
}
