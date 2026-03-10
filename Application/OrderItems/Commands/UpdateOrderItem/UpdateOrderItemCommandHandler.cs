using Application.OrderItems.Commands.CreateOrderItem;
using Application.OrderItems.Commands.DeleteOrderItem;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.OrderItems.Commands.UpdateOrderItem;

public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand>
{
    private readonly IOrderItemsRepository _orderItemRepository;
    private readonly IValidator<UpdateOrderItemCommand> _validator;
    private readonly IMapper<UpdateOrderItemCommand, OrderItem> _mapper;

    public UpdateOrderItemCommandHandler(IOrderItemsRepository orderItemRepository, IValidator<UpdateOrderItemCommand> validator, IMapper<UpdateOrderItemCommand, OrderItem> mapper)
    {
        _orderItemRepository = orderItemRepository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task Handle(UpdateOrderItemCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return;
        }

        var oi = _mapper.Map(request);

        await _orderItemRepository.UpdateAsync(oi, cancellationToken);
    }
}
