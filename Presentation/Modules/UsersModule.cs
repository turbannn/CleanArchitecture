using Application.Users.Commands.CreateUser;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetUserById;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Modules;

public class UsersModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {

        app.MapGet("/Users/GetById", async (Guid id, ISender sender, CancellationToken cancellation) =>
        {
            var query = new GetUserByIdQuery(id);

            var order = await sender.Send(query, cancellation);

            return Results.Ok(order);
        });

        app.MapPost("/Users/Create", async (CreateUserCommand request, ISender sender, CancellationToken cancellation) =>
        {
            await sender.Send(request, cancellation);
            return Results.Ok();
        });

        app.MapPut("/Users/Update", async (UpdateUserCommand request, ISender sender, CancellationToken cancellation) =>
        {
            await sender.Send(request, cancellation);
            return Results.Ok();
        });

        app.MapDelete("/Users/DeleteById", async (Guid id, ISender sender, CancellationToken cancellation) =>
        {
            var query = new DeleteUserCommand(id);

            await sender.Send(query, cancellation);

            return Results.Ok();
        });
    }
}
