using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(Guid Id, string Username, string Password) : IRequest;
