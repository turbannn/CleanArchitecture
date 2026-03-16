using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Domain;

public class AssemblyReference
{
    public static Assembly Assembly { get; private set; } = typeof(AssemblyReference).Assembly;
}
