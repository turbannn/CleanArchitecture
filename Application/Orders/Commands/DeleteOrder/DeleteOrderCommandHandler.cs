using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.DeleteOrder;

public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, GlovoResult>
{
    private readonly IOrdersRepository _orderRepository;
    private readonly IValidator<DeleteOrderCommand> _validator;

    public DeleteOrderCommandHandler(IOrdersRepository orderRepository, IValidator<DeleteOrderCommand> validator)
    {
        _orderRepository = orderRepository;
        _validator = validator;
    }

    public async Task<GlovoResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return GlovoResult.Fail(res.Errors.First().ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var deleteRes = await _orderRepository.DeleteAsync(request.Id, cancellationToken);

        if (!deleteRes)
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        else
            return GlovoResult.Success();
    }
}