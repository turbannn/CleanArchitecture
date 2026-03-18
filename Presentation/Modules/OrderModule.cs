using Application.Orders.Commands.CreateOrder;
using Application.Orders.Commands.DeleteOrder;
using Application.Orders.Commands.UpdateOrder;
using Application.Orders.Queries.GetOrderById;
using Carter;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using Presentation.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Modules;

public class OrderModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/Orders/GetById", async (Guid id, ISender sender, ILogger<OrderModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("GET /Orders/GetById for {OrderId}", id);
            var query = new GetOrderByIdQuery(id);

            var result = await sender.Send(query, cancellation);

            logger.LogInformation("GET /Orders/GetById completed for {OrderId} with success {IsSuccess} and code {StatusCode}", id, result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });

        app.MapPost("/Orders/Create", async (CreateOrderCommand request, ISender sender, ILogger<OrderModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("POST /Orders/Create for user {UserId}", request.UserId);
            var result = await sender.Send(request, cancellation);

            logger.LogInformation("POST /Orders/Create completed with success {IsSuccess} and code {StatusCode}", result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });

        app.MapPut("/Orders/Update", async (UpdateOrderCommand request, ISender sender, ILogger<OrderModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("PUT /Orders/Update for order {OrderId}", request.Id);
            var result =await sender.Send(request, cancellation);

            logger.LogInformation("PUT /Orders/Update completed for order {OrderId} with success {IsSuccess} and code {StatusCode}", request.Id, result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });

        app.MapDelete("/Orders/DeleteById", async (Guid id, ISender sender, ILogger<OrderModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("DELETE /Orders/DeleteById for order {OrderId}", id);
            var query = new DeleteOrderCommand(id);

            var result = await sender.Send(query, cancellation);

            logger.LogInformation("DELETE /Orders/DeleteById completed for order {OrderId} with success {IsSuccess} and code {StatusCode}", id, result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });
    }
}
