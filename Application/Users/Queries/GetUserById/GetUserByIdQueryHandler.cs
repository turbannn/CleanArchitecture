using Domain.Entities;
using Domain.Interfaces;
using Domain.Results;
using Domain.Utilities;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Application.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, GenericGlovoResult<User>>
{
    private readonly IUsersRepository _repository;
    private readonly IValidator<GetUserByIdQuery> _validator;

    public GetUserByIdQueryHandler(IUsersRepository repository, IValidator<GetUserByIdQuery> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<GenericGlovoResult<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var res = await _validator.ValidateAsync(request);

        if (!res.IsValid)
        {
            Console.WriteLine(res.Errors.First());
            return GenericGlovoResult<User>.Fail(res.Errors.First().ErrorMessage, GlovoStatusCodes.BadRequest);
        }

        var user = await _repository.GetByIdAsync(request.Id, cancellationToken);

        if(user is null)
            return GenericGlovoResult<User>.Fail("User was not found", GlovoStatusCodes.NotFound);

        return GenericGlovoResult<User>.Success(user);
    }
}
