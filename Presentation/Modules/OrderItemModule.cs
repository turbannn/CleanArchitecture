using Application.OrderItems.Commands.CreateOrderItem;
using Application.OrderItems.Queries.GetOrderItemById;
using Carter;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Presentation.Modules;

public class OrderItemModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        
        app.MapGet("/OrderItems/GetById", async ([FromQuery] Guid id, ISender sender, CancellationToken cancellation) =>
        {
            var query = new GetOrderItemByIdQuery(id);

            var item = await sender.Send(query, cancellation);

            return Results.Ok(item);
        });
        

        app.MapPost("/OrderItems/Create", async ([FromBody] CreateOrderItemCommand request, ISender sender, CancellationToken cancellation) =>
        {
            await sender.Send(request, cancellation);

            return Results.Ok();
        });
    }
}
