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
    }
}
