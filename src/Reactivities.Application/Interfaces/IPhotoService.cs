using Microsoft.AspNetCore.Http;
using Reactivities.Application.Helpers;
using System.Threading.Tasks;

namespace Reactivities.Application.Interfaces
{
    public interface IPhotoService
    {
        Task<PhotoUploadResult> AddPhoto(IFormFile file);
        Task<string> DeletePhoto(string publicId);
    }
}
