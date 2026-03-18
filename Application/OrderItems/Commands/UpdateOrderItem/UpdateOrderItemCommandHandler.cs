using Application.OrderItems.Commands.CreateOrderItem;
using Application.OrderItems.Commands.DeleteOrderItem;
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

namespace Application.OrderItems.Commands.UpdateOrderItem;

public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand, GlovoResult>
{
    private readonly IOrderItemsRepository _orderItemRepository;
    private readonly IValidator<UpdateOrderItemCommand> _validator;
    private readonly IMapper<UpdateOrderItemCommand, OrderItem> _mapper;
    private readonly ILogger<UpdateOrderItemCommandHandler> _logger;

    public UpdateOrderItemCommandHandler(IOrderItemsRepository orderItemRepository, IValidator<UpdateOrderItemCommand> validator, IMapper<UpdateOrderItemCommand, OrderItem> mapper, ILogger<UpdateOrderItemCommandHandler> logger)
    {
        _orderItemRepository = orderItemRepository;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GlovoResult> Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateOrderItemCommand for item {OrderItemId}", request.Id);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("UpdateOrderItemCommand validation failed for item {OrderItemId}: {Error}", request.Id, firstError.ErrorMessage);
            return GlovoResult.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var oi = _mapper.Map(request);

        var updateRes = await _orderItemRepository.UpdateAsync(oi, cancellationToken);

        if(!updateRes)
        {
            _logger.LogError("Failed to update order item {OrderItemId}", request.Id);
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        }

        _logger.LogInformation("Order item {OrderItemId} updated successfully", request.Id);
        return GlovoResult.Success();

    }
}
