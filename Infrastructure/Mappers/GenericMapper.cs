using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Mappers;

public class GenericMapper<TypeIn, TypeOut> : IMapper<TypeIn, TypeOut>
{
    public TypeOut Map(TypeIn input)
    {
        var output = Activator.CreateInstance<TypeOut>();

        var type = typeof(TypeIn);

        var props = type.GetProperties().ToList();

        foreach(var p in props)
        {
            var value = p.GetValue(input);
            var outputProperty = typeof(TypeOut).GetProperty(p.Name);
            if (outputProperty != null && outputProperty.PropertyType.Equals(p.PropertyType))
            {
                outputProperty.SetValue(output, value);
            }
        }

        return output;
    }
}
