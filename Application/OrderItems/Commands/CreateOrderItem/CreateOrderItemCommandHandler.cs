using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.CreateOrderItem;

public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand, GlovoResult>
{
    private readonly IOrderItemsRepository _orderItemRepository;
    private readonly IOrdersRepository _ordersRepository;
    private readonly IValidator<CreateOrderItemCommand> _validator;
    private readonly IMapper<CreateOrderItemCommand, OrderItem> _mapper;

    public CreateOrderItemCommandHandler(IOrderItemsRepository orderItemRepository, 
        IOrdersRepository ordersRepository, 
        IValidator<CreateOrderItemCommand> validator, 
        IMapper<CreateOrderItemCommand, OrderItem> mapper)
    {
        _orderItemRepository = orderItemRepository;
        _ordersRepository = ordersRepository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<GlovoResult> Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return GlovoResult.Fail(res.Errors.First().ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var oi = await _ordersRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (oi is null)
            GlovoResult.Fail("Order item was not found", GlovoStatusCodes.NotFound);

        var oi2 = _mapper.Map(request);
        oi2.Id = Guid.NewGuid();

        var createRes = await _orderItemRepository.AddAsync(oi2, cancellationToken);

        if (!createRes)
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        else
            return GlovoResult.Success();
    }
}
