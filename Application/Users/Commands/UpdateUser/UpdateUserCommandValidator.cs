using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        //Id
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("User Id must not be empty.")
            .Must(id => Guid.TryParse(id.ToString(), out _)).WithMessage("User Id must be a valid GUID.");

        //String
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username must not be empty.")
            .MaximumLength(70).WithMessage("Username must not exceed 70 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password must not be empty.")
            .MaximumLength(120).WithMessage("Password must not exceed 120 characters");
    }
}
