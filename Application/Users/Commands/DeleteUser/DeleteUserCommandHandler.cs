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

namespace Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, GlovoResult>
{
    private readonly IUsersRepository _repository;
    private readonly IValidator<DeleteUserCommand> _validator;
    private readonly ILogger<DeleteUserCommandHandler> _logger;
    public DeleteUserCommandHandler(IUsersRepository repository, IValidator<DeleteUserCommand> validator, ILogger<DeleteUserCommandHandler> logger)
    {
        _repository = repository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<GlovoResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling DeleteUserCommand for user {UserId}", request.Id);
        var res = await _validator.ValidateAsync(request, cancellationToken);

        if (!res.IsValid)
        {
            var firstError = res.Errors.First();
            _logger.LogWarning("DeleteUserCommand validation failed for user {UserId}: {Error}", request.Id, firstError.ErrorMessage);
            return GlovoResult.Fail(firstError.ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var deleteRes = await _repository.DeleteAsync(request.Id, cancellationToken);

        if (!deleteRes)
        {
            _logger.LogError("Failed to delete user {UserId}", request.Id);
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        }

        _logger.LogInformation("User {UserId} deleted successfully", request.Id);
        return GlovoResult.Success();
    }
}
