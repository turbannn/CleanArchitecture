using Application.OrderItems.Commands.CreateOrderItem;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.DeleteOrderItem;

public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, GlovoResult>
{
    private readonly IOrderItemsRepository _orderItemsRepository;
    private readonly IValidator<DeleteOrderItemCommand> _validator;
    private readonly ILogger<DeleteOrderItemCommandHandler> _logger;

    public DeleteOrderItemCommandHandler(IOrderItemsRepository orderItemsRepository, IValidator<DeleteOrderItemCommand> validator, ILogger<DeleteOrderItemCommandHandler> logger)
    {
        _orderItemsRepository = orderItemsRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<GlovoResult> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteOrderItemCommand for item {OrderItemId}", request.Id);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("DeleteOrderItemCommand validation failed for item {OrderItemId}: {Error}", request.Id, firstError.ErrorMessage);
            return GlovoResult.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var delRes = await _orderItemsRepository.DeleteAsync(request.Id, cancellationToken);

        if (!delRes)
        {
            _logger.LogError("Failed to delete order item {OrderItemId}", request.Id);
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        }

        _logger.LogInformation("Order item {OrderItemId} deleted successfully", request.Id);
        return GlovoResult.Success();
    }
}
