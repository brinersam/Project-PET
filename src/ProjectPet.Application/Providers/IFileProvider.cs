﻿using CSharpFunctionalExtensions;
using ProjectPet.Domain.Shared;

namespace ProjectPet.Application.Providers
{
    public interface IFileProvider
    {
        Task<Result<List<FileInfoDto>, Error>> GetFilesAsync(
            string bucket,
            int userId,
            CancellationToken cancellationToken = default);

        Task<Result<List<string>, Error>> UploadFilesAsync(
            IEnumerable<FileDataDto> dataList,
            string bucket,
            int userId,
            CancellationToken cancellationToken = default);
    }
}
