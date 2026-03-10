using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.CreateUser;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
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
    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return;
        }

        var user = _mapper.Map(request);
        user.Id = Guid.NewGuid();

        await _repository.AddAsync(user, cancellationToken);
    }
}
