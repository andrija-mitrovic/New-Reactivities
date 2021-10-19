using MediatR;
using Microsoft.AspNetCore.Http;
using Reactivities.Application.Helpers;
using Reactivities.Domain.Entities;

namespace Reactivities.Application.Features.Photos.Commands
{
    public class CreatePhotoCommand : IRequest<Result<Photo>>
    {
        public IFormFile File { get; set; }
    }
}
