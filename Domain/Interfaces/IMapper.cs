using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces;

public interface IMapper<TypeIn, TypeOut>
{
    TypeOut Map(TypeIn input);
}
