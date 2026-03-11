using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, GlovoResult>
{
    private readonly IUsersRepository _repository;
    private readonly IValidator<UpdateUserCommand> _validator;
    private readonly IMapper<UpdateUserCommand, User> _mapper;
    public UpdateUserCommandHandler(IUsersRepository repository, IValidator<UpdateUserCommand> validator, IMapper<UpdateUserCommand, User> mapper)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
    }

    public async Task<GlovoResult> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return GlovoResult.Fail(res.Errors.First().ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var user = _mapper.Map(request);

        var updateRes = await _repository.UpdateAsync(user, cancellationToken);

        if (!updateRes)
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        else
            return GlovoResult.Success();
    }
}
