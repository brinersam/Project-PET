using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using ProjectPet.Application.Providers;
using ProjectPet.Domain.Shared;
using ProjectPet.Infrastructure.Options;

namespace ProjectPet.Infrastructure.Providers
{
    public class MinioProvider : IFileProvider
    {
        private readonly IMinioClient _minioClient;
        private readonly ILogger<MinioProvider> _logger;
        private readonly IOptions<OptionsMinIO> _options;

        public MinioProvider(
            IMinioClient minioClient,
            ILogger<MinioProvider> logger,
            IOptions<OptionsMinIO> options)
        {
            _minioClient = minioClient;
            _logger = logger;
            _options = options;
        }

        public async Task<Result<List<string>, Error>> UploadFilesAsync(
            IEnumerable<FileDataDto> dataList,
            string bucket,
            int userId,
            CancellationToken cancellationToken = default)
        {
            var semaphore = new SemaphoreSlim(_options.Value.MaxConcurrentUpload);
            List<string> uploadedFilePaths = [];

            string userBucket = GetBucketName(bucket, userId);
            var createBucketRes = await CreateBucketIfMissing(userBucket, cancellationToken);
            if (createBucketRes.IsFailure)
                return createBucketRes.Error;

            var putObjectTasks = dataList.Select(async file => await PutObject(
                                                        userBucket,
                                                        semaphore,
                                                        file,
                                                        cancellationToken));

            var taskResults = await Task.WhenAll(putObjectTasks);

            var isContainsErrors = taskResults.Any(x => x.IsFailure);
            if (isContainsErrors)
                return taskResults.First(x => x.IsFailure).Error;

            List<string> uploadedFilePaths = taskResults
                                .Select(task => task.Value)
                                .ToList();

            _logger.LogInformation("Successfully uploaded files {files} to a bucket {bucket}",
                String.Join(" | ", uploadedFilePaths),
                userBucket);

            return uploadedFilePaths;
        }

        private async Task<Result<string,Error>> PutObject(
            string targetBucket,
            SemaphoreSlim semaphore,
            FileDataDto file,
            CancellationToken cancellationToken)
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(targetBucket)
                    .WithStreamData(file.Stream)
                    .WithObjectSize(file.Stream.Length)
                    .WithObject(file.ObjectName);

                var putRes = await _minioClient.PutObjectAsync(
                    putObjectArgs,
                    cancellationToken);

                return putRes.ObjectName;
            }
            catch (Exception ex)
            {
                return ErrorFailure(ex.Message);
            }
            finally
            {
                semaphore.Release();
            }
        }

        private async Task<UnitResult<Error>> CreateBucketIfMissing(
            string bucketName,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketExistsArgs = new BucketExistsArgs()
                    .WithBucket(bucketName);

                var bucketExists = await _minioClient
                    .BucketExistsAsync(bucketExistsArgs, cancellationToken);

                var bucketExists = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);
                if (bucketExists == false)
                {
                    var makeBucketArgs = new MakeBucketArgs()
                        .WithBucket(bucketName);

                    await _minioClient.MakeBucketAsync(makeBucketArgs);
                }

                return Result.Success<Error>();
            }
            catch (Exception ex)
            {
                return ErrorFailure(ex.Message);
            }
        }

        private Error ErrorFailure(string msg)
        => Error.Failure("minio.failure", $"Minio errored: {msg}");

        private static string GetBucketName(string bucket, int userId)
            => $"id{userId}.{bucket}";
    }
}
