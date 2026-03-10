using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(string Username, string Password) : IRequest;
