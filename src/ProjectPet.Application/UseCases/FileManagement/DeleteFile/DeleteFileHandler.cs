using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Application.Providers;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.FileManagement
{
    public class DeleteFileHandler
    {
        private const string BUCKETNAME = "photos";

        private readonly IFileProvider _fileProvider;
        private readonly ILogger<DeleteFileHandler> _logger;

        public DeleteFileHandler(
            IFileProvider fileProvider,
            ILogger<DeleteFileHandler> logger)
        {
            _fileProvider = fileProvider;
            _logger = logger;
        }

        public async Task<UnitResult<Error>> Handle(
            DeleteFileRequest request,
            CancellationToken cancellationToken = default)
        {
            var deleteFilesResult = await _fileProvider.DeleteFilesAsync(
                BUCKETNAME,
                request.DebugUserId,
                request.FilesToDelete,
                cancellationToken);

            if (deleteFilesResult.IsFailure)
                return deleteFilesResult.Error;

            _logger.LogInformation("Successfully deleted files for user with id {userid}", request.DebugUserId);
            return Result.Success<Error>();
        }
    }
}
