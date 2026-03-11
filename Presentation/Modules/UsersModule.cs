using Application.Users.Commands.CreateUser;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetUserById;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Presentation.Extensions;
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

            var result = await sender.Send(query, cancellation);

            return result.ToHttpResult();
        });

        app.MapPost("/Users/Create", async (CreateUserCommand request, ISender sender, CancellationToken cancellation) =>
        {
            var result = await sender.Send(request, cancellation);

            return result.ToHttpResult();
        });

        app.MapPut("/Users/Update", async (UpdateUserCommand request, ISender sender, CancellationToken cancellation) =>
        {
            var result = await sender.Send(request, cancellation);

            return result.ToHttpResult();
        });

        app.MapDelete("/Users/DeleteById", async (Guid id, ISender sender, CancellationToken cancellation) =>
        {
            var query = new DeleteUserCommand(id);

            var result = await sender.Send(query, cancellation);

            return result.ToHttpResult();
        });
    }
}
