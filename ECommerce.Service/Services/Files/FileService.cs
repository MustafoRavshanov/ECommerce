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

    public async Task<ResponseModel<string>> UploadFileAsync(Stream fileStream, string contentType)
    {
        var bucketExists = await minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(bucketName));

        if (!bucketExists)
        {
            await minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucketName));
        }

        var uniqueFileName = $"{Guid.NewGuid()}";

        await minioClient.PutObjectAsync(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(uniqueFileName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType));

        var fileData = new FileData
        {
            EndPoint = endpoint,
            FileLength = fileStream.Length,
            ContentType = contentType,
            BasketName= bucketName,
            UniqueFileName = uniqueFileName
        };

        await applicationDbContext.AddAsync(fileData);
        var result = await applicationDbContext.SaveChangesAsync();

        if(result< 1)
            return ResponseModel<string>.Fail("Failed to save file data to the database", HttpStatusCode.InternalServerError);

        return ResponseModel<string>.Success(uniqueFileName, "File uploaded successfully", HttpStatusCode.OK);
    }

    public async Task<ResponseModel<string>> GetUrlByIdAsync(int id)
    { 
        var fileData = await applicationDbContext.FileDatas.FirstOrDefaultAsync(f => f.Id == id);

        if(fileData is null)
            return ResponseModel<string>.Fail("File not found", HttpStatusCode.NotFound);

        var result= $"https://{fileData.EndPoint}/{fileData.BasketName}/{fileData.UniqueFileName}";

        return ResponseModel<string>.Success(result, "File URL retrieved successfully", HttpStatusCode.OK);
    }

    public async Task<TableResponse<List<string>>> GetAllUrlAsync(TableOptions options)
    {
        var fileDatas = applicationDbContext.FileDatas.AsQueryable();
            

        var count = await fileDatas.CountAsync();

        var fileDatasList = await fileDatas
            .Skip(options.First)
            .Take(options.Rows)
            .ToListAsync();

        var result = fileDatasList.Select(f => $"https://{f.EndPoint}/{f.BasketName}/{f.UniqueFileName}").ToList();

        return new TableResponse<List<string>>() {Total = count, Items = result };
    }
}