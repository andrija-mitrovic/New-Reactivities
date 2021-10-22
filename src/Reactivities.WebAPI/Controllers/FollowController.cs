using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Features.Followers.Commands;
using System.Threading.Tasks;

namespace Reactivities.WebAPI.Controllers
{
    public class FollowController : BaseApiController
    {
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await Mediator.Send(new FollowToggleCommand { TargetUsername = username }));
        }
    }
}
