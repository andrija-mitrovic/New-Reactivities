using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Features.Followers.Commands;
using Reactivities.Application.Features.Followers.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.WebAPI.Controllers
{
    public class FollowController : BaseApiController
    {
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new FollowToggleCommand { TargetUsername = username }, cancellationToken));
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollowings(string username, string predicate, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new GetFollowersListQuery
            {
                Username = username,
                Predicate = predicate
            }, cancellationToken));
        }
    }
}
