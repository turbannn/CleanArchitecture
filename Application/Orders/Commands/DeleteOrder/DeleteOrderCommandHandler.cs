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

namespace Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, GlovoResult>
{
    private readonly IOrdersRepository _orderRepository;
    private readonly IValidator<DeleteOrderCommand> _validator;
    private readonly ILogger<DeleteOrderCommandHandler> _logger;

    public DeleteOrderCommandHandler(IOrdersRepository orderRepository, IValidator<DeleteOrderCommand> validator, ILogger<DeleteOrderCommandHandler> logger)
    {
        _orderRepository = orderRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<GlovoResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteOrderCommand for order {OrderId}", request.Id);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("DeleteOrderCommand validation failed for order {OrderId}: {Error}", request.Id, firstError.ErrorMessage);
            return GlovoResult.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var deleteRes = await _orderRepository.DeleteAsync(request.Id, cancellationToken);

        if (!deleteRes)
        {
            _logger.LogError("Failed to delete order {OrderId}", request.Id);
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        }

        _logger.LogInformation("Order {OrderId} deleted successfully", request.Id);
        return GlovoResult.Success();
    }
}