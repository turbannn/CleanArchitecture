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

namespace Application.OrderItems.Commands.CreateOrderItem;

public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, GlovoResult>
{
    private readonly IOrderItemsRepository _orderItemRepository;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IValidator<CreateOrderItemCommand> _validator;
    private readonly IMapper<CreateOrderItemCommand, OrderItem> _mapper;
    private readonly ILogger<CreateOrderItemCommandHandler> _logger;

    public CreateOrderItemCommandHandler(IOrderItemsRepository orderItemRepository, 
        IOrdersRepository ordersRepository, 
        IValidator<CreateOrderItemCommand> validator, 
        IMapper<CreateOrderItemCommand, OrderItem> mapper,
        ILogger<CreateOrderItemCommandHandler> logger)
    {
        _orderItemRepository = orderItemRepository;
        _ordersRepository = ordersRepository;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GlovoResult> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateOrderItemCommand for order {OrderId}", request.OrderId);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("CreateOrderItemCommand validation failed for order {OrderId}: {Error}", request.OrderId, firstError.ErrorMessage);
            return GlovoResult.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var oi = await _ordersRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (oi is null)
        {
            _logger.LogWarning("Order {OrderId} not found while creating order item", request.OrderId);
            return GlovoResult.Fail("Order item was not found", GlovoStatusCodes.NotFound);
        }

        var oi2 = _mapper.Map(request);
        oi2.Id = Guid.NewGuid();

        var createRes = await _orderItemRepository.AddAsync(oi2, cancellationToken);

        if (!createRes)
        {
            _logger.LogError("Failed to persist order item {OrderItemId}", oi2.Id);
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        }

        _logger.LogInformation("Order item {OrderItemId} created successfully", oi2.Id);
        return GlovoResult.Success();
    }
}
