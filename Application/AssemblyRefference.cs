using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Application
{
    public static class AssemblyRefference
    {
        public static Assembly Assembly { get; private set; } = typeof(AssemblyRefference).Assembly;
    }
}