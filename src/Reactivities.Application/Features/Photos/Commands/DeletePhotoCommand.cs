using MediatR;
using Reactivities.Application.Helpers;

namespace Reactivities.Application.Features.Photos.Commands
{
    public class DeletePhotoCommand : IRequest<Result<Unit>>
    {
        public string Id { get; set; }
    }
}
