using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand>
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

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return;
        }

        var user = _mapper.Map(request);

        await _repository.UpdateAsync(user, cancellationToken);
    }
}
