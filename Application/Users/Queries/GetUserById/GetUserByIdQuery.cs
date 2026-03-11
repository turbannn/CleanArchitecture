using Domain.Entities;
using Domain.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Queries.GetUserById
{
    public sealed record GetUserByIdQuery(Guid Id) : IRequest<GenericGlovoResult<User>>;
}
