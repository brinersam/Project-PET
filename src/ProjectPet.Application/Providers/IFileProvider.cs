using CSharpFunctionalExtensions;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.Providers
{
    public interface IFileProvider
    {
        Task<Result<List<string>, Error>> UploadFilesAsync(
            IEnumerable<FileDataDto> dataList,
            string targetBucket,
            CancellationToken cancellationToken = default);
    }
}
