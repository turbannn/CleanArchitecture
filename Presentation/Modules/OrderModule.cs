using Application.Orders.Commands.CreateOrder;
using Application.Orders.Commands.DeleteOrder;
using Application.Orders.Commands.UpdateOrder;
using Application.Orders.Queries.GetOrderById;
using Carter;
using MediatR;
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
        app.MapGet("/Orders/GetById", async (Guid id, ISender sender, CancellationToken cancellation) =>
        {
            var query = new GetOrderByIdQuery(id);

            var result = await sender.Send(query, cancellation);

            return result.ToHttpResult();
        });

        app.MapPost("/Orders/Create", async (CreateOrderCommand request, ISender sender, CancellationToken cancellation) =>
        {
            var result = await sender.Send(request, cancellation);

            return result.ToHttpResult();
        });

        app.MapPut("/Orders/Update", async (UpdateOrderCommand request, ISender sender, CancellationToken cancellation) =>
        {
            var result =await sender.Send(request, cancellation);

            return result.ToHttpResult();
        });

        app.MapDelete("/Orders/DeleteById", async (Guid id, ISender sender, CancellationToken cancellation) =>
        {
            var query = new DeleteOrderCommand(id);

            var result = await sender.Send(query, cancellation);

            return result.ToHttpResult();
        });
    }
}
