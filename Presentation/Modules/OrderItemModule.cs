using Application.OrderItems.Commands.CreateOrderItem;
using Application.OrderItems.Commands.DeleteOrderItem;
using Application.OrderItems.Commands.UpdateOrderItem;
using Application.OrderItems.Queries.GetOrderItemById;
using Carter;
using Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Presentation.Extensions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Presentation.Modules;

public class OrderItemModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        
        app.MapGet("/OrderItems/GetById", async ([FromQuery] Guid id, ISender sender, ILogger<OrderItemModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("GET /OrderItems/GetById for {OrderItemId}", id);
            var query = new GetOrderItemByIdQuery(id);

            var result = await sender.Send(query, cancellation);

            logger.LogInformation("GET /OrderItems/GetById completed for {OrderItemId} with success {IsSuccess} and code {StatusCode}", id, result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });
        

        app.MapPost("/OrderItems/Create", async ([FromBody] CreateOrderItemCommand request, ISender sender, ILogger<OrderItemModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("POST /OrderItems/Create for order {OrderId}", request.OrderId);
            var result = await sender.Send(request, cancellation);

            logger.LogInformation("POST /OrderItems/Create completed with success {IsSuccess} and code {StatusCode}", result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });

        app.MapPut("/OrderItems/Update", async ([FromBody] UpdateOrderItemCommand request, ISender sender, ILogger<OrderItemModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("PUT /OrderItems/Update for order item {OrderItemId}", request.Id);
            var result = await sender.Send(request, cancellation);

            logger.LogInformation("PUT /OrderItems/Update completed for order item {OrderItemId} with success {IsSuccess} and code {StatusCode}", request.Id, result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });

        app.MapDelete("/OrderItems/DeleteById", async ([FromQuery] Guid id, ISender sender, ILogger<OrderItemModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("DELETE /OrderItems/DeleteById for order item {OrderItemId}", id);
            var query = new DeleteOrderItemCommand(id);

            var result = await sender.Send(query, cancellation);

            logger.LogInformation("DELETE /OrderItems/DeleteById completed for order item {OrderItemId} with success {IsSuccess} and code {StatusCode}", id, result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });
    }
}
