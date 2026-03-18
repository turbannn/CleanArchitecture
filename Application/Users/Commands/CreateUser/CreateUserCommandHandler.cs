using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, GlovoResult>
{
    private readonly IUsersRepository _repository;
    private readonly IValidator<CreateUserCommand> _validator;
    private readonly IMapper<CreateUserCommand, User> _mapper;
    private readonly ILogger<CreateUserCommandHandler> _logger;
    public CreateUserCommandHandler(IUsersRepository repository, IValidator<CreateUserCommand> validator, IMapper<CreateUserCommand, User> mapper, ILogger<CreateUserCommandHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<GlovoResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling CreateUserCommand for username {Username}", request.Username);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("CreateUserCommand validation failed for username {Username}: {Error}", request.Username, firstError.ErrorMessage);
            return GlovoResult.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var user = _mapper.Map(request);
        user.Id = Guid.NewGuid();

        var createRes = await _repository.AddAsync(user, cancellationToken);

        if (!createRes)
        {
            _logger.LogError("Failed to persist user {UserId}", user.Id);
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        }

        _logger.LogInformation("User {UserId} created successfully", user.Id);
        return GlovoResult.Success();
    }
}
