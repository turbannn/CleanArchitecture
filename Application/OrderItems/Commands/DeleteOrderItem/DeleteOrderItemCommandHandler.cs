using Application.OrderItems.Commands.CreateOrderItem;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.DeleteOrderItem;

public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, GlovoResult>
{
    private readonly IOrderItemsRepository _orderItemsRepository;
    private readonly IValidator<DeleteOrderItemCommand> _validator;

    public DeleteOrderItemCommandHandler(IOrderItemsRepository orderItemsRepository, IValidator<DeleteOrderItemCommand> validator)
    {
        _orderItemsRepository = orderItemsRepository;
        _validator = validator;
    }

    public async Task<GlovoResult> Handle(DeleteOrderItemCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return GlovoResult.Fail(res.Errors.First().ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var delRes = await _orderItemsRepository.DeleteAsync(request.Id, cancellationToken);

        if (!delRes)
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        else
            return GlovoResult.Success();
    }
}
