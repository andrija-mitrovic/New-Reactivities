using MediatR;
using Reactivities.Application.Helpers;

namespace Reactivities.Application.Features.Photos.Commands
{
    public class SetMainPhotoCommand : IRequest<Result<Unit>>
    {
        public string Id { get; set; }
    }
}
