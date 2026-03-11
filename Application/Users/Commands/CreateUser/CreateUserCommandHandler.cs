using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, GlovoResult>
{
    private readonly IUsersRepository _repository;
    private readonly IValidator<CreateUserCommand> _validator;
    private readonly IMapper<CreateUserCommand, User> _mapper;
    public CreateUserCommandHandler(IUsersRepository repository, IValidator<CreateUserCommand> validator, IMapper<CreateUserCommand, User> mapper)
    {
        _repository = repository;
        _validator = validator;
        _mapper = mapper;
    }
    public async Task<GlovoResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return GlovoResult.Fail(res.Errors.First().ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var user = _mapper.Map(request);
        user.Id = Guid.NewGuid();

        var createRes = await _repository.AddAsync(user, cancellationToken);

        if (!createRes)
            return GlovoResult.Fail("Internal server error", GlovoStatusCodes.InternalServerError);
        else
            return GlovoResult.Success();
    }
}
