using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, GlovoResult>
{
    private readonly IUsersRepository _repository;
    private readonly IValidator<DeleteUserCommand> _validator;
    public DeleteUserCommandHandler(IUsersRepository repository, IValidator<DeleteUserCommand> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<GlovoResult> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return GlovoResult.Fail(res.Errors.First().ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var deleteRes = await _repository.DeleteAsync(request.Id, cancellationToken);

        if (!deleteRes)
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        else
            return GlovoResult.Success();
    }
}
