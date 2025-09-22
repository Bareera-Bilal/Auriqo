using System;
using Auriqo_Web_Api_Backend.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;

namespace Auriqo_Web_Api_Backend.Services;

public class CloudinaryService : ICloudinaryService
{

    private readonly Cloudinary cloudinary;

    // private readonly string  url;
    public CloudinaryService()
    {
        // this.cloudinary = cloudinary;

        // this.url = configuration["Cloudinary:CLOUDINARY_URL"] ?? throw new ArgumentNullException("Cloudinary Url is not configured.");


        try
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
        }
        catch (System.Exception ex)
        {

            throw new InvalidOperationException("Failed to load .env file.", ex);
        }


        var CloudinaryUrl = Environment.GetEnvironmentVariable("CLOUDINARY_URL");

        this.cloudinary = new Cloudinary(CloudinaryUrl) { Api = { Secure = true } };


    }



    public async Task<string> UploadImageAsync(IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            throw new ArgumentException("Image is missing.");
        }

        using var stream = image.OpenReadStream();


        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(image.FileName, stream),
            UseFilename = true,
            UniqueFilename = false,
            Overwrite = true,
            Folder = "Trinkle"
            // Transformation = new Transformation().Width(150).Height(150).Crop("fill")
        };

        var uploadResult = await cloudinary.UploadAsync(uploadParams);

        if (uploadResult.Error != null)
        {
            throw new InvalidOperationException($"Cloudinary upload failed: {uploadResult.Error.Message}");
        }

        return uploadResult.SecureUrl.ToString();
    }

    public Task<string> UploadMultipleImageAsync(IFormFile imgArr)
    {
        throw new NotImplementedException();
    }

    public async Task<string> UploadVideoAsync(IFormFile video)
    {
        try
        {
            if (video == null || video.Length == 0)
            {
                throw new ArgumentException("video is missing.");
            }


            using var stream = video.OpenReadStream();


            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(video.FileName, stream),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true,
                Folder = "Trinkle"
            };

            var uploadResult = await cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
            {
                throw new InvalidOperationException($"Cloudinary upload failed: {uploadResult.Error.Message}");
            }


            return uploadResult.SecureUrl.ToString();



        }
        catch (System.Exception)
        {

            throw;
        }
    }
}