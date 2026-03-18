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

namespace Application.OrderItems.Queries.GetOrderItemById;

public class GetOrderItemByIdQueryHandler : IRequestHandler<GetOrderItemByIdQuery, GenericGlovoResult<OrderItem>>
{
    private readonly IOrderItemsRepository _orderItemRepository;
    private readonly IValidator<GetOrderItemByIdQuery> _validator;
    private readonly ILogger<GetOrderItemByIdQueryHandler> _logger;

    public GetOrderItemByIdQueryHandler(IOrderItemsRepository orderItemRepository, IValidator<GetOrderItemByIdQuery> validator, ILogger<GetOrderItemByIdQueryHandler> logger)
    {
        _orderItemRepository = orderItemRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<GenericGlovoResult<OrderItem>> Handle(GetOrderItemByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetOrderItemByIdQuery for item {OrderItemId}", request.Id);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("GetOrderItemByIdQuery validation failed for item {OrderItemId}: {Error}", request.Id, firstError.ErrorMessage);
            return GenericGlovoResult<OrderItem>.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var orderItem = await _orderItemRepository.GetByIdAsync(request.Id, cancellationToken);

        if (orderItem is null)
        {
            _logger.LogWarning("Order item {OrderItemId} not found", request.Id);
            return GenericGlovoResult<OrderItem>.Fail("Order item was not found", GlovoStatusCodes.NotFound);
        }

        _logger.LogInformation("Order item {OrderItemId} retrieved successfully", request.Id);
        return GenericGlovoResult<OrderItem>.Success(orderItem);
    }
}