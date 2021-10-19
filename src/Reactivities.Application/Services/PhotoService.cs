using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace Reactivities.Application.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<PhotoService> _logger;

        public PhotoService(IOptions<CloudinarySettings> config,
            ILogger<PhotoService> logger)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
            _logger = logger;
        }

        public async Task<PhotoUploadResult> AddPhoto(IFormFile file)
        {
            _logger.LogInformation("PhotoService.AddPhoto - Adding photo to Cloudinary.");

            if (file.Length > 0)
            {
                await using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error != null)
                {
                    throw new Exception(uploadResult.Error.Message);
                }

                _logger.LogInformation("PhotoService.AddPhoto - Photo added successfully to Cloudinary.");

                return new PhotoUploadResult
                {
                    PublicId = uploadResult.PublicId,
                    Url = uploadResult.SecureUrl.ToString()
                };
            }

            _logger.LogInformation("PhotoService.AddPhoto - File empty.");
            return null;
        }

        public async Task<string> DeletePhoto(string publicId)
        {
            _logger.LogInformation("PhotoService.DeletePhoto - Deleting photo from Cloudinary.");

            var deleteParams = new DeletionParams(publicId);
            var result = await _cloudinary.DestroyAsync(deleteParams);

            if(result.Result == "ok")
            {
                _logger.LogInformation("PhotoService.DeletePhoto - Photo deleted successfully from Cloudinary.");
                return result.Result;
            }

            _logger.LogInformation("PhotoService.DeletePhoto - Failed to delete photo from cloudinary");
            return null;
        }
    }
}
