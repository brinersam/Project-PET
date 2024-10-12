﻿using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using ProjectPet.Application.Database;
using ProjectPet.Application.Providers;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.UseCases.FileManagement.UploadFile
{
    public class UploadFileHandler
    {
        private const string BUCKETNAME = "photos";

        private readonly ILogger<UploadFileHandler> _logger;
        private readonly IFileProvider _fileProvider;
        private readonly IUnitOfWork _unitOfWork;

        public UploadFileHandler(
            ILogger<UploadFileHandler> logger,
            IFileProvider fileProvider,
            IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _fileProvider = fileProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<string>, Error>> HandleAsync(
            UploadFileRequest request,
            CancellationToken cancellationToken = default)
        {
            var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                //var volunteerIdRes = await _repository.GetByIdAsync(request.Id);
                //if (volunteerIdRes.IsFailure)
                //    return volunteerIdRes.Errors;
                var volunteerId = request.DebugId; // we use debug id, always succeed

                var fileList = request.Files.Select(file =>
                    new FileDataDto(
                        file.Stream,
                        $"{request.Title}_{Guid.NewGuid()}{Path.GetExtension(file.FilePath)}")
                    )
                    .ToList();

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                var fileuploadRes = await _fileProvider.UploadFilesAsync(
                    fileList,
                    BUCKETNAME,
                    cancellationToken);

                if (fileuploadRes.IsSuccess == false)
                    return fileuploadRes.Error;

                transaction.Commit();

                return fileuploadRes.Value;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return Error.Failure("failure", ex.Message);
            }
        }
    }
}
