using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, GenericGlovoResult<Order>>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IValidator<GetOrderByIdQuery> _validator;

    public GetOrderByIdQueryHandler(IOrdersRepository ordersRepository, IValidator<GetOrderByIdQuery> validator)
    {
        _ordersRepository = ordersRepository;
        _validator = validator;
    }

    public async Task<GenericGlovoResult<Order>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return GenericGlovoResult<Order>.Fail(res.Errors.First().ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var order = await _ordersRepository.GetByIdAsync(request.Id, cancellationToken);

        if (order is null)
            return GenericGlovoResult<Order>.Fail("Order was not found", GlovoStatusCodes.NotFound);

        return GenericGlovoResult<Order>.Success(order);
    }
}
