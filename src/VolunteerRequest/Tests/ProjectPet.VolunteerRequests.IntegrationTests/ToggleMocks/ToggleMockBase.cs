using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ClearExtensions;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

public abstract class ToggleMockBase<TInterface, TImplementation>
    where TInterface : class
    where TImplementation : class, TInterface
{
    private HashSet<string> _mockedFunctionNames = new();

    public TInterface Mock { get; protected set; }
    public IServiceProvider ServiceProvider { get; private set; } = null!;

    protected ToggleMockBase()
    {
        Mock = Substitute.For<TInterface>();
    }

    public void SetProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public void MockFunction(string nameOfFunction)
    {
        _mockedFunctionNames.Add(nameOfFunction);
    }

    public void Reset()
    {
        Mock.ClearSubstitute();
        _mockedFunctionNames.Clear();
    }

    protected bool IsMocked(string functionName)
        => _mockedFunctionNames.Contains(functionName);

    protected TInterface CreateInstance()
        => (TImplementation)ActivatorUtilities.CreateInstance(ServiceProvider, typeof(TImplementation));
}
