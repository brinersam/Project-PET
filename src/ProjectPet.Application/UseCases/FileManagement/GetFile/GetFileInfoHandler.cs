using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Application.Providers;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.FileManagement;

public class GetFileInfoHandler
{
    private const string BUCKETNAME = "photos";

    private readonly IFileProvider _fileProvider;
    private readonly ILogger<GetFileInfoHandler> _logger;

    public GetFileInfoHandler(
        IFileProvider fileProvider,
        ILogger<GetFileInfoHandler> logger)
    {
        _fileProvider = fileProvider;
        _logger = logger;
    }

    public async Task<Result<List<FileInfoDto>, Error>> Handle(
        GetFileRequest request,
        CancellationToken cancellationToken)
    {
        var getFilesRes = await _fileProvider.GetFilesAsync(
            BUCKETNAME,
            request.DebugUserId,
            cancellationToken);

        if (getFilesRes.IsFailure)
            return getFilesRes.Error;

        _logger.LogInformation("Successfully retrieved {numfiles} files for user with id {userid}", getFilesRes.Value.Count, request.DebugUserId);
        return getFilesRes.Value;
    }
}
