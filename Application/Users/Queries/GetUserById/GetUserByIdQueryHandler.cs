using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, User>
{
    private readonly IUsersRepository _repository;
    private readonly IValidator<GetUserByIdQuery> _validator;
    public GetUserByIdQueryHandler(IUsersRepository repository, IValidator<GetUserByIdQuery> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<User> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return new User { Username = "null" };
        }

        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if(user is null)
            throw new ArgumentNullException("User is null!");

        return user;
    }
}
