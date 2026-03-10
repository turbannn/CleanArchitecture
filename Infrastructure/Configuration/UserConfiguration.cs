using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Id).IsRequired();
        builder.Property(u => u.Username).IsRequired().HasMaxLength(70);
        builder.Property(u => u.Password).IsRequired().HasMaxLength(120);
    }
}
