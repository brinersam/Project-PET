using CSharpFunctionalExtensions;
using ProjectPet.Core.Providers;
using ProjectPet.SharedKernel.ErrorClasses;

namespace ProjectPet.Core.Abstractions;

public interface IFileProvider
{
    string FileNamer(params string[] args);

    Task<UnitResult<Error>> DeleteFilesAsync(
        string bucket,
        Guid userId,
        IEnumerable<string> fileKeys,
        CancellationToken cancellationToken = default);

    Task<Result<List<FileInfoDto>, Error>> GetFilesAsync(
        string bucket,
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<Result<List<string>, Error>> UploadFilesAsync(
        IEnumerable<FileDataDto> dataList,
        string bucket,
        Guid userId,
        CancellationToken cancellationToken = default);
}
