using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Features.Profiles.Queries;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.WebAPI.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new GetProfileByUsernameQuery { Username = username }, cancellationToken));
        }

        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetUserActivities(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new GetListActivityQuery { Username = username, Predicate = predicate }));
        }
    }
}
