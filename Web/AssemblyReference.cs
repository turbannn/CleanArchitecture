using System.Reflection;

namespace Web;

public class AssemblyReference
{
    public static Assembly Assembly { get; private set; } = typeof(AssemblyReference).Assembly;
}
