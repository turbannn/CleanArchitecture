using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.CreateOrderItem;

public class CreateOrderItemCommandHandler : IRequestHandler<CreateOrderItemCommand>
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

    public async Task Handle(CreateOrderItemCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return;
        }

        var oi = await _ordersRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if(oi is null)
            throw new NullReferenceException();

        var oi2 = _mapper.Map(request);

        oi2.Id = Guid.NewGuid();

        await _orderItemRepository.AddAsync(oi2, cancellationToken);
    }
}
