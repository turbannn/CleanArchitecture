using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;

    public List<Order> Orders { get; set; } = null!;
}
