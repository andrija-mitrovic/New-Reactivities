using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Features.Photos.Commands;
using System.Threading.Tasks;

namespace Reactivities.WebAPI.Controllers
{
    public class PhotosController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreatePhotoCommand command)
        {
            return HandleResult(await Mediator.Send(command));
        }
    }
}
