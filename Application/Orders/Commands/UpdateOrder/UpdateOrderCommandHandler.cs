using Application.Orders.Queries.GetOrderById;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Orders.Commands.UpdateOrder;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, GlovoResult>
{
    private readonly IOrdersRepository _ordersRepository;
    private readonly IValidator<UpdateOrderCommand> _validator;
    private readonly IMapper<UpdateOrderCommand, Order> _mapper;


    public UpdateOrderCommandHandler(IOrdersRepository ordersRepository, IValidator<UpdateOrderCommand> validator, IMapper<UpdateOrderCommand, Order> mapper)
    {
        _ordersRepository = ordersRepository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<GlovoResult> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return GlovoResult.Fail(res.Errors.First().ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var order = _mapper.Map(request);

        var updateRes = await _ordersRepository.UpdateAsync(order, cancellationToken);

        if (!updateRes)
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        else
            return GlovoResult.Success();
    }
}
