using Domain.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Presentation.Extensions;

public static class ResultsExtensions
{
    public static IResult ToHttpResult<T>(this GenericGlovoResult<T> result)
    {
        if (result.IsSuccess)
            return Results.Ok(result);

        switch (result.Code)
        {
            case 400:
                return Results.BadRequest(result);
            case 401:
                return Results.Unauthorized();
            case 404:
                return Results.NotFound(result);
            case 500:
                return Results.InternalServerError(result);
            default:
                return Results.BadRequest(result);
        }
    }

    public static IResult ToHttpResult(this GlovoResult result)
    {
        if (result.IsSuccess)
            return Results.Ok(result);

        switch (result.Code)
        {
            case 400:
                return Results.BadRequest(result);
            case 401:
                return Results.Unauthorized();
            case 404:
                return Results.NotFound(result);
            case 500:
                return Results.InternalServerError(result);
            default:
                return Results.BadRequest(result);
        }
    }
}
