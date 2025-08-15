using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NSubstitute.ClearExtensions;
using System.Diagnostics.CodeAnalysis;

public abstract class ToggleMockBase
{
    public IServiceProvider ServiceProvider { get; private set; } = null!;
    public Type ReplacedType { get; init; }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "sybau")]
    protected HashSet<string> _mockedFunctionNames = new();

    public void SetProvider(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public void MockFunction(string nameOfFunction)
    {
        _mockedFunctionNames.Add(nameOfFunction);
    }

    abstract public void Reset();

    protected bool IsMocked(string functionName)
        => _mockedFunctionNames.Contains(functionName);
}

public abstract class ToggleMockBase<TInterface, TImplementation> : ToggleMockBase
    where TInterface : class
    where TImplementation : class, TInterface
{
    public TInterface Mock { get; protected set; }

    protected ToggleMockBase()
    {
        ReplacedType = typeof(TInterface);
        Mock = Substitute.For<TInterface>();
    }

    override public void Reset()
    {
        Mock.ClearSubstitute();
        _mockedFunctionNames.Clear();
    }

    protected TInterface CreateInstance() 
        => (TImplementation)ActivatorUtilities.CreateInstance(ServiceProvider, typeof(TImplementation));
    // todo
    // maybe on registering the mock itself, we first switch the original service to keyed service
    // replace the original registration with togglemock
    // and then here we retreive it from DI instead so we respect it origianlly being transient / singleton
}
