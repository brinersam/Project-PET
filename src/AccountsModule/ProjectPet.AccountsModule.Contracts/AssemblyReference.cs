using System.Reflection;

namespace ProjectPet.AccountsModule.Contracts;
public static class AssemblyReference
{
    public static Assembly Assembly => typeof(AssemblyReference).Assembly;
}
