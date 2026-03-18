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

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, GlovoResult>
{
    private readonly IUsersRepository _repository;
    private readonly IValidator<UpdateUserCommand> _validator;
    private readonly IMapper<UpdateUserCommand, User> _mapper;
    private readonly ILogger<UpdateUserCommandHandler> _logger;
    public UpdateUserCommandHandler(IUsersRepository repository, IValidator<UpdateUserCommand> validator, IMapper<UpdateUserCommand, User> mapper, ILogger<UpdateUserCommandHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<GlovoResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling UpdateUserCommand for user {UserId}", request.Id);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("UpdateUserCommand validation failed for user {UserId}: {Error}", request.Id, firstError.ErrorMessage);
            return GlovoResult.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var user = _mapper.Map(request);

        var updateRes = await _repository.UpdateAsync(user, cancellationToken);

        if (!updateRes)
        {
            _logger.LogError("Failed to update user {UserId}", request.Id);
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        }

        _logger.LogInformation("User {UserId} updated successfully", request.Id);
        return GlovoResult.Success();
    }
}
