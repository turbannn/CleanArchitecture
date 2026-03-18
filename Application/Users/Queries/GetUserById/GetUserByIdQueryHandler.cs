using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GenericGlovoResult<User>>
{
    private readonly IUsersRepository _repository;
    private readonly IValidator<GetUserByIdQuery> _validator;
    private readonly ILogger<GetUserByIdQueryHandler> _logger;

    public GetUserByIdQueryHandler(IUsersRepository repository, IValidator<GetUserByIdQuery> validator, ILogger<GetUserByIdQueryHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<GenericGlovoResult<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetUserByIdQuery for user {UserId}", request.Id);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("GetUserByIdQuery validation failed for user {UserId}: {Error}", request.Id, firstError.ErrorMessage);
            return GenericGlovoResult<User>.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if(user is null)
        {
            _logger.LogWarning("User {UserId} not found", request.Id);
            return GenericGlovoResult<User>.Fail("User was not found", GlovoStatusCodes.NotFound);
        }

        _logger.LogInformation("User {UserId} retrieved successfully", request.Id);
        return GenericGlovoResult<User>.Success(user);
    }
}
