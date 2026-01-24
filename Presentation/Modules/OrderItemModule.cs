using Application.OrderItems.Commands.CreateOrderItem;
using Application.OrderItems.Queries.GetOrderItemById;
using Carter;
using Domain.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Modules;

public class OrderItemModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/OrderItems/GetById", async ([FromQuery] Guid id, GetOrderItemByIdQueryHandler handler) =>
        {
            var query = new GetOrderItemByIdQuery(id);

            var item = await handler.Handle(query);

            return Results.Ok(item);
        });

        app.MapPost("/OrderItems/Create", async ([FromBody] CreateOrderItemCommand request, CreateOrderItemCommandHandler handler) =>
        {
            await handler.Handle(request);

            return Results.Ok();
        });
    }
}
