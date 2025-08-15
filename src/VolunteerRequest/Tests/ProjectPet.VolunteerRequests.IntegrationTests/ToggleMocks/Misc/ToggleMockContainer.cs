using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace ProjectPet.VolunteerRequests.IntegrationTests.ToggleMocks.Misc;
public class ToggleMockContainer
{
    protected Dictionary<Type, ToggleMockBase> _mocks = new();

    public ToggleMockBase this[Type type]
    {
        get => _mocks[type];
        init => _mocks[type] = value;
    }

    public static object GetMock(Type mockType)
    {
        var method = typeof(ToggleMockContainer)
            .GetMethod(nameof(GetMock), BindingFlags.NonPublic | BindingFlags.Static)
            !.MakeGenericMethod(mockType);

        return method.Invoke(null, Array.Empty<object>())!;
    }

    public T GetMock<T>()
        where T : ToggleMockBase
    {
        var b = _mocks[typeof(T)];
        return (T)b;
    }

    public void SetMockProviders(IServiceProvider provider)
    {
        foreach (var mock in _mocks.Values)
            mock.SetProvider(provider);
    }

    public void ResetMocks()
    {
        foreach (var mock in _mocks.Values)
            mock.Reset();
    }

    public void InjectToggleMocks(IServiceCollection services)
    {
        foreach (var (mockType, mock) in _mocks)
            services.Replace(ServiceDescriptor.Scoped(mock.ReplacedType, _ => _mocks[mockType]));
    }
}
