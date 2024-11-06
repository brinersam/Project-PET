using CSharpFunctionalExtensions;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using ProjectPet.Application.Providers;
using ProjectPet.Domain.Shared;
using ProjectPet.Infrastructure.Options;
using System.Reactive.Linq;

namespace ProjectPet.Infrastructure.Providers;

public class MinioProvider : IFileProvider
{
    private readonly IMinioClient _minioClient;
    private readonly IOptions<OptionsMinIO> _options;

    public MinioProvider(
        IMinioClient minioClient,
        IOptions<OptionsMinIO> options)
    {
        _minioClient = minioClient;
        _options = options;
    }

    public async Task<Result<List<string>, Error>> UploadFilesAsync(
        IEnumerable<FileDataDto> dataList,
        string bucket,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var semaphore = new SemaphoreSlim(_options.Value.MaxConcurrentUpload);
        string userBucket = GetBucketName(bucket, userId);

        if (await BucketExistsAsync(userBucket, cancellationToken) == false)
        {
            var createBucketRes = await CreateBucket(userBucket, cancellationToken);
            if (createBucketRes.IsFailure)
                return createBucketRes.Error;
        }

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

        return uploadedFilePaths;
    }

    public async Task<Result<List<FileInfoDto>, Error>> GetFilesAsync(
        string bucket,
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        string userBucket = GetBucketName(bucket, userId);

        if (await BucketExistsAsync(userBucket, cancellationToken) == false)
            return ErrorMissingBucket(userBucket);

        var objectNamesRes = await GetObjectsFromBucketAsync(userBucket, cancellationToken);
        if (objectNamesRes.IsFailure)
            return objectNamesRes.Error;

        var queryRes = await ObjectNamesToUrlPairs(userBucket, objectNamesRes.Value);
        if (queryRes.IsFailure)
            return queryRes.Error;

        return queryRes.Value;
    }

    public async Task<UnitResult<Error>> DeleteFilesAsync(
        string bucket,
        Guid userId,
        IEnumerable<string> fileKeys,
        CancellationToken cancellationToken = default)
    {
        string userBucket = GetBucketName(bucket, userId);

        if (await BucketExistsAsync(userBucket, cancellationToken) == false)
            return ErrorMissingBucket(userBucket);

        try
        {
            var objectDeletionArgs = new RemoveObjectsArgs().WithBucket(userBucket).WithObjects(fileKeys.ToList());
            var objectDeletionRes = await _minioClient.RemoveObjectsAsync(objectDeletionArgs, cancellationToken);

        }
        catch (Exception ex)
        {
            return ErrorFailure(ex.Message);
        }

        return Result.Success<Error>();
    }

    private async Task<Result<List<FileInfoDto>, Error>> ObjectNamesToUrlPairs(
    string bucketName,
    IEnumerable<string> objectNames)
    {
        List<FileInfoDto> result = [];
        foreach (string oName in objectNames)
        {
            try
            {
                PresignedGetObjectArgs args = new PresignedGetObjectArgs()
                                                  .WithBucket(bucketName)
                                                  .WithObject(oName)
                                                  .WithExpiry(60 * 30); // sec

                var url = await _minioClient.PresignedGetObjectAsync(args);
                result.Add(new FileInfoDto(oName, url));
            }
            catch (Exception ex)
            {
                return ErrorFailure(ex.Message);
            }
        }

        return result;
    }

    private async Task<Result<List<string>, Error>> GetObjectsFromBucketAsync(
        string userBucket,
        CancellationToken cancellationToken = default)
    {
        List<string> result = [];

        var args = new ListObjectsArgs()
                        .WithBucket(userBucket)
                        .WithRecursive(false);
        try
        {
            await _minioClient.ListObjectsAsync(args, cancellationToken)
                .ForEachAsync(o => result.Add(o.Key));
        }
        catch (Exception ex)
        {
            return ErrorFailure(ex.Message);
        }

        return result;
    }

    private async Task<Result<string, Error>> PutObject(
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

    private async Task<UnitResult<Error>> CreateBucket(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(bucketName);

            await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);

            return Result.Success<Error>();
        }
        catch (Exception ex)
        {
            return ErrorFailure(ex.Message);
        }
    }

    private async Task<bool> BucketExistsAsync(
        string bucketName,
        CancellationToken cancellationToken = default)
    {
        var bucketExistsArgs = new BucketExistsArgs()
                .WithBucket(bucketName);
        try
        {
            return await _minioClient.BucketExistsAsync(
                bucketExistsArgs,
                cancellationToken);
        }
        catch (Minio.Exceptions.BucketNotFoundException)
        {
            return false;
        }
    }

    private Error ErrorMissingBucket(string bucketName)
    => Error.NotFound("minio.missing.bucket", $"Requested bucket {bucketName} is not found!");

    private Error ErrorFailure(string msg)
=> Error.Failure("minio.failure", $"Minio errored: {msg}");

    private static string GetBucketName(string bucket, Guid userId)
        => $"id{userId}.{bucket}";
}
