using Application.OrderItems.Commands.CreateOrderItem;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tests.OrderItem.Commands;

public class CreateOrderItemCommandTests
{
    private readonly Mock<IOrderItemsRepository> _orderItemsRepositoryMock;
    private readonly Mock<IOrdersRepository> _ordersRepositoryMock;
    private readonly Mock<IValidator<CreateOrderItemCommand>> _createOrderItemCommandValidatorMock;
    private readonly Mock<IMapper<CreateOrderItemCommand, Domain.Entities.OrderItem>> _createOrderItemCommandMapperMock;
    private readonly Mock<ILogger<CreateOrderItemCommandHandler>> _createOrderItemCommandHandlerLoggerMock;

    public CreateOrderItemCommandTests()
    {
        _orderItemsRepositoryMock = new();
        _ordersRepositoryMock = new();
        _createOrderItemCommandValidatorMock = new();
        _createOrderItemCommandMapperMock = new();
        _createOrderItemCommandHandlerLoggerMock = new();
    }

    [Test]
    public async Task Handle_Should_Create_OrderItem()
    {
        // Arrange
        var command = new CreateOrderItemCommand(10.0m, "Test product", 5, "n-t", Guid.NewGuid());
        
        _orderItemsRepositoryMock.Setup(
            repo => repo.AddAsync(
                It.IsAny<Domain.Entities.OrderItem>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _ordersRepositoryMock.Setup(
            ordersRepo => ordersRepo.GetByIdAsync(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Order { Id = command.OrderId});

        _createOrderItemCommandValidatorMock.Setup(
            validator => validator.ValidateAsync(
                It.IsAny<CreateOrderItemCommand>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult() { Errors = new() });

        _createOrderItemCommandMapperMock.Setup(
            mapper => mapper.Map(
                It.IsAny<CreateOrderItemCommand>()))
            .Returns(
            new Domain.Entities.OrderItem()
            { 
                Id = Guid.NewGuid(),
                UnitPrice = command.UnitPrice,
                ProductName = command.ProductName,
                Quantity = command.Quantity,
                StockKeepingUnit = command.StockKeepingUnit,
                OrderId = command.OrderId
            });

        var handler = new CreateOrderItemCommandHandler(
            _orderItemsRepositoryMock.Object,
            _ordersRepositoryMock.Object,
            _createOrderItemCommandValidatorMock.Object,
            _createOrderItemCommandMapperMock.Object, 
            _createOrderItemCommandHandlerLoggerMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Error, Is.Null);
        Assert.That(result.IsSuccess, Is.True);
    }
}
