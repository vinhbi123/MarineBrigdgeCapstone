using Garage.Domain.Models.Settings;
using ShipCapstone.Application.Services.Interfaces;
using ShipCapstone.Domain.Models.Settings;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;

namespace ShipCapstone.Application.Services.Implements;

public class UploadService : IUploadService
{
    private readonly ILogger _logger;
    private readonly S3CompatibleStorageSettings _compatibleStorageSettings;
    
    public UploadService(ILogger logger, IOptions<S3CompatibleStorageSettings> compatibleStorageSettings) 
    {
        _logger = logger;
        _compatibleStorageSettings = compatibleStorageSettings.Value;
    }
    
    public async Task<string> UploadImageAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            throw new BadHttpRequestException("Không tìm thấy file");
        }
        
        var allowedExtensions = new[] { ".jpeg", ".png", ".jpg", ".gif", ".bmp", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            throw new InvalidOperationException(
                "Chỉ các định dạng tệp txt, .pdf, .doc, .docx, .xls, .xlsx, .ppt, và .pptx được phép tải lên.");
        try
        {
            var minio = new MinioClient()
                .WithEndpoint(_compatibleStorageSettings.EndPoint)
                .WithCredentials(_compatibleStorageSettings.AccessKey, _compatibleStorageSettings.SecretKey)
                .Build();

            var headers = new Dictionary<string, string>
            {
                { "x-amz-acl", "public-read" }
            };
            var objectName = $"{Guid.NewGuid().ToString()}{extension}";
            var result = await minio.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_compatibleStorageSettings.BucketName)
                .WithObject(objectName)
                .WithStreamData(file.OpenReadStream())
                .WithObjectSize(file.Length)
                .WithContentType("image/jpeg")
                .WithHeaders(headers)
            );
            if (result == null)
                throw new MinioException("Failed to upload image");
            
            return $"https://{_compatibleStorageSettings.EndPoint}/{_compatibleStorageSettings.BucketName}/{objectName}";
        }
        catch (Exception e)
        {
            _logger.Error($"Failed to upload image: {e.Message}");
            throw new Exception("Failed to upload image", e);
        }
    }
}