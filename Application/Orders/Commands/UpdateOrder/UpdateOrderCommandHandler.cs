using Application.Orders.Queries.GetOrderById;
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

namespace Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, GlovoResult>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IValidator<UpdateOrderCommand> _validator;
    private readonly IMapper<UpdateOrderCommand, Order> _mapper;
    private readonly ILogger<UpdateOrderCommandHandler> _logger;


    public UpdateOrderCommandHandler(IOrdersRepository ordersRepository, IValidator<UpdateOrderCommand> validator, IMapper<UpdateOrderCommand, Order> mapper, ILogger<UpdateOrderCommandHandler> logger)
    {
        _ordersRepository = ordersRepository;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GlovoResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateOrderCommand for order {OrderId}", request.Id);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("UpdateOrderCommand validation failed for order {OrderId}: {Error}", request.Id, firstError.ErrorMessage);
            return GlovoResult.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var order = _mapper.Map(request);

        var updateRes = await _ordersRepository.UpdateAsync(order, cancellationToken);

        if (!updateRes)
        {
            _logger.LogError("Failed to update order {OrderId}", request.Id);
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        }

        _logger.LogInformation("Order {OrderId} updated successfully", request.Id);
        return GlovoResult.Success();
    }
}
