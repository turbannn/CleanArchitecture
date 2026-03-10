using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User Id must not be empty.")
            .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("User Id must be a valid GUID.");
    }
}
