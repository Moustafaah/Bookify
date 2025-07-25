using System.Reflection;

namespace Infrastructure;
internal class AssemblyRef
{
    public static Assembly Assembly => Assembly.GetExecutingAssembly();
}
