using Application.Orders.Commands.CreateOrder;
using Application.Orders.Queries.GetOrderById;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Modules;

public class OrderModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/Orders/GetById", async (Guid id, ISender sender, CancellationToken cancellation) =>
        {
            var query = new GetOrderByIdQuery(id);

            var order = await sender.Send(query, cancellation);

            return order;
        });

        app.MapPost("/Orders/Create", async (CreateOrderCommand request, ISender sender, CancellationToken cancellation) =>
        {
            await sender.Send(request, cancellation);
            return Results.Ok();
        });
    }
}
