using ECommerce.Service.Services.MinIO;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Service.Services.Files;

public class FileService : IFileService
{
    private readonly IMinioClient _minioClient;
    private readonly string _bucketName;
    private readonly string _endpoint;

    public FileService(IConfiguration configuration)
    {
        _endpoint = configuration["MinIO:Endpoint"]!;
        _bucketName = configuration["MinIO:BucketName"]!;

        _minioClient = new MinioClient()
            .WithEndpoint(_endpoint)
            .WithCredentials(configuration["MinIO:AccessKey"], configuration["MinIO:SecretKey"])
            .Build();
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType)
    {
        var bucketExists = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_bucketName));

        if (!bucketExists)
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_bucketName));
        }

        var uniqueFileName = $"{Guid.NewGuid()}";

        await _minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_bucketName)
            .WithObject(uniqueFileName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType));

        return $"http://{_endpoint}/{_bucketName}/{uniqueFileName}";
    }
}