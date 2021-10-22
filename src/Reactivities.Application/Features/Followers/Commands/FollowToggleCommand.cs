using MediatR;
using Reactivities.Application.Helpers;

namespace Reactivities.Application.Features.Followers.Commands
{
    public class FollowToggleCommand : IRequest<Result<Unit>>
    {
        public string TargetUsername { get; set; }
    }
}
