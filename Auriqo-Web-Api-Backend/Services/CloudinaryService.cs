using System;
using Auriqo_Web_Api_Backend.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;




namespace Auriqo_Web_Api_Backend.Services;

public class CloudinaryService : ICloudinaryService
{


    private readonly Cloudinary cloudinary;

    public CloudinaryService( string cloudinaryUrl)
    {

        this.cloudinary = new Cloudinary(cloudinaryUrl);

        cloudinary.Api.Secure = true;
    }


    public async Task<string> UploadImageAsync(IFormFile image, string folderName)
    {
        if (image == null || image.Length == 0)
        {
            throw new ArgumentNullException("File is null");
        }

        using var stream = image.OpenReadStream();


        var uploadParams = new ImageUploadParams()
        {

            File = new FileDescription(image.FileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true,
            Folder = folderName
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        return uploadResult.SecureUrl.ToString();



    }

    public async Task<string> UploadVideoAsync(IFormFile video, string folderName)
    {
        try
        {
            if (video == null || video.Length == 0)
            {
                throw new ArgumentNullException("File is null");
            }

            using var stream = video.OpenReadStream();

            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(video.FileName, stream),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true,
                Folder = folderName

            };
            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUrl.ToString();


        }
        catch (System.Exception)
        {

            throw;
        }
    }

    public async Task<string> UploadImageAsync(Stream stream, string fileName, string folderName)
    {
        if (stream == null || stream.Length == 0)
            throw new ArgumentException("Image stream is null or empty", nameof(stream));

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true,
            Folder = folderName
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception($"Cloudinary image upload failed: {uploadResult.Error?.Message}");

        return uploadResult.SecureUrl.ToString();
    }

    public async Task<string> UploadVideoAsync(Stream stream, string fileName, string folderName)
    {
        if (stream == null || stream.Length == 0)
            throw new ArgumentException("Video stream is null or empty", nameof(stream));

        var uploadParams = new VideoUploadParams
        {
            File = new FileDescription(fileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true,
            Folder = folderName
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception($"Cloudinary video upload failed: {uploadResult.Error?.Message}");

        return uploadResult.SecureUrl.ToString();
    }

}