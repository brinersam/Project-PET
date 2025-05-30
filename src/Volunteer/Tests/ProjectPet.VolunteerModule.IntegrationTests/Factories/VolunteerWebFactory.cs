using CSharpFunctionalExtensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using ProjectPet.SharedKernel.ErrorClasses;
using ProjectPet.Core.Files;
using ProjectPet.Core.Database;

namespace ProjectPet.VolunteerModule.IntegrationTests.Factories;
public class VolunteerWebFactory : IntegrationTestWebFactoryBase
{
    protected readonly IFileProvider _fileProviderMock = Substitute.For<IFileProvider>();

    protected override void ConfigureServices(IServiceCollection services)
    {
        base.ConfigureServices(services);
        services.Replace(ServiceDescriptor.Scoped(typeof(IFileProvider), _ => _fileProviderMock));
        services.RemoveAll<IDatabaseSeeder>();
    }

    protected override string[] IncludedDBSchemas()
        => ["volunteer"];

    #region Arrange methods
    public int IFileProviderMock_UploadFilesAsync_Success()
    {
        var result = Result.Success<List<string>, Error>
                ([
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString(),
                    Guid.NewGuid().ToString()
                ]);

        _fileProviderMock
            .UploadFilesAsync(
                Arg.Any<IEnumerable<FileDataDto>>(), Arg.Any<string>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()
            )
            .Returns(result);

        return result.Value.Count;
    }

    public void IFileProviderMock_UploadFilesAsync_Failure()
    {
        var result = Result.Failure<List<string>, Error>(
            Error.Failure("debug_error_code", "debug_error_message"));

        _fileProviderMock
            .UploadFilesAsync(
                Arg.Any<IEnumerable<FileDataDto>>(), Arg.Any<string>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()
            )
            .Returns(result);
    }
    #endregion
}
