using ECommerce.Domain.Entities;
using ECommerce.Domain.Helper;
using ECommerce.Infrastructure.Data;
using ECommerce.Service.Services.MinIO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Minio;
using Minio.DataModel.Args;
using System.Net;

namespace ECommerce.Service.Services.Files;

public class FileService (ApplicationDbContext applicationDbContext): IFileService
{
    private readonly IMinioClient minioClient;
    private readonly string bucketName;
    private readonly string endpoint;

    public FileService(IConfiguration configuration, ApplicationDbContext dbContext): this(dbContext)
    {
        endpoint = configuration["MinIO:Endpoint"]!;
        bucketName = configuration["MinIO:BucketName"]!;

        minioClient = new MinioClient()
            .WithEndpoint(endpoint)
            .WithCredentials(configuration["MinIO:AccessKey"], configuration["MinIO:SecretKey"])
            .Build();
    }

    public async Task<ResponseModel<string>> UploadFileAsync(Stream fileStream, string contentType, string fileSign)
    {
        var existingFile = await applicationDbContext.FileDatas.FirstOrDefaultAsync(f => f.FileName == fileSign);

        if (existingFile is not null)
        { 
            return ResponseModel<string>.Fail("File with the same name already exists");
        }

        var bucketExists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName));

        if (!bucketExists)
        {
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName));
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{fileSign}";

        await minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(uniqueFileName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType));

        var fileData = new FileData
        {
            EndPoint = endpoint,
            FileName = fileSign,
            UniqueFileName = uniqueFileName,
            FileLength = fileStream.Length,
            ContentType = contentType,
            BasketName= bucketName
        };

        await applicationDbContext.AddAsync(fileData);
        var result = await applicationDbContext.SaveChangesAsync();

        if(result< 1)
            return ResponseModel<string>.Fail("Failed to save file data to the database", HttpStatusCode.InternalServerError);

        return ResponseModel<string>.Success(uniqueFileName, "File uploaded successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<string>> GetFileUrlByName(string fileSign)
    { 
        var fileData = await applicationDbContext.FileDatas.FirstOrDefaultAsync(f => f.FileName == fileSign);

        if (fileData is null)
            return ResponseModel<string>.Fail("File not found", HttpStatusCode.NotFound);

        var result = $"{fileData.EndPoint}/{fileData.BasketName}/{fileData.UniqueFileName}";

        return ResponseModel<string>.Success(result, "File URL retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<List<string>>> FilterUrlsByName(string name)
    {
        var fileDatas = await applicationDbContext.FileDatas
            .Where(f => f.FileName.StartsWith(name))
            .ToListAsync();

        if (fileDatas.Count == 0)
            return ResponseModel<List<string>>.Fail("No files found with the given name", HttpStatusCode.NotFound);

        var urls = fileDatas.Select(f => $"{f.EndPoint}/{f.BasketName}/{f.UniqueFileName}").ToList();

        return ResponseModel<List<string>>.Success(urls,"All Urls retrieved successfully", HttpStatusCode.OK);
    }
}