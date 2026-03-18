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

namespace Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, GenericGlovoResult<Order>>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IValidator<GetOrderByIdQuery> _validator;
    private readonly ILogger<GetOrderByIdQueryHandler> _logger;

    public GetOrderByIdQueryHandler(IOrdersRepository ordersRepository, IValidator<GetOrderByIdQuery> validator, ILogger<GetOrderByIdQueryHandler> logger)
    {
        _ordersRepository = ordersRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<GenericGlovoResult<Order>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetOrderByIdQuery for order {OrderId}", request.Id);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("GetOrderByIdQuery validation failed for order {OrderId}: {Error}", request.Id, firstError.ErrorMessage);
            return GenericGlovoResult<Order>.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var order = await _ordersRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
        {
            _logger.LogWarning("Order {OrderId} not found", request.Id);
            return GenericGlovoResult<Order>.Fail("Order was not found", GlovoStatusCodes.NotFound);
        }

        _logger.LogInformation("Order {OrderId} retrieved successfully", request.Id);
        return GenericGlovoResult<Order>.Success(order);
    }
}
