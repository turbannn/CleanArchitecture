using Application.Users.Commands.CreateUser;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.UpdateUser;
using Application.Users.Queries.GetUserById;
using Carter;
using MediatR;
using Microsoft.Extensions.Logging;
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

        app.MapGet("/Users/GetById", async (Guid id, ISender sender, ILogger<UsersModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("GET /Users/GetById for user {UserId}", id);
            var query = new GetUserByIdQuery(id);

            var result = await sender.Send(query, cancellation);

            logger.LogInformation("GET /Users/GetById completed for user {UserId} with success {IsSuccess} and code {StatusCode}", id, result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });

        app.MapPost("/Users/Create", async (CreateUserCommand request, ISender sender, ILogger<UsersModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("POST /Users/Create for username {Username}", request.Username);
            var result = await sender.Send(request, cancellation);

            logger.LogInformation("POST /Users/Create completed with success {IsSuccess} and code {StatusCode}", result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });

        app.MapPut("/Users/Update", async (UpdateUserCommand request, ISender sender, ILogger<UsersModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("PUT /Users/Update for user {UserId}", request.Id);
            var result = await sender.Send(request, cancellation);

            logger.LogInformation("PUT /Users/Update completed for user {UserId} with success {IsSuccess} and code {StatusCode}", request.Id, result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });

        app.MapDelete("/Users/DeleteById", async (Guid id, ISender sender, ILogger<UsersModule> logger, CancellationToken cancellation) =>
        {
            logger.LogInformation("DELETE /Users/DeleteById for user {UserId}", id);
            var query = new DeleteUserCommand(id);

            var result = await sender.Send(query, cancellation);

            logger.LogInformation("DELETE /Users/DeleteById completed for user {UserId} with success {IsSuccess} and code {StatusCode}", id, result.IsSuccess, result.Code);

            return result.ToHttpResult();
        });
    }
}
