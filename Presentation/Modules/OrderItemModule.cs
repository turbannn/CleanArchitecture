using Application.OrderItems.Commands.CreateOrderItem;
using Application.OrderItems.Commands.DeleteOrderItem;
using Application.OrderItems.Commands.UpdateOrderItem;
using Application.OrderItems.Queries.GetOrderItemById;
using Carter;
using Domain.Interfaces;
using MediatR;
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
        
        app.MapGet("/OrderItems/GetById", async ([FromQuery] Guid id, ISender sender, CancellationToken cancellation) =>
        {
            var query = new GetOrderItemByIdQuery(id);

            var result = await sender.Send(query, cancellation);

            return result.ToHttpResult();
        });
        

        app.MapPost("/OrderItems/Create", async ([FromBody] CreateOrderItemCommand request, ISender sender, CancellationToken cancellation) =>
        {
            var result = await sender.Send(request, cancellation);

            return result.ToHttpResult();
        });

        app.MapPut("/OrderItems/Update", async ([FromBody] UpdateOrderItemCommand request, ISender sender, CancellationToken cancellation) =>
        {
            var result = await sender.Send(request, cancellation);

            return result.ToHttpResult();
        });

        app.MapDelete("/OrderItems/DeleteById", async ([FromQuery] Guid id, ISender sender, CancellationToken cancellation) =>
        {
            var query = new DeleteOrderItemCommand(id);

            var result = await sender.Send(query, cancellation);

            return result.ToHttpResult();
        });
    }
}
