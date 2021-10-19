using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Features.Photos.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.WebAPI.Controllers
{
    public class PhotosController : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] CreatePhotoCommand command, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(command, cancellationToken));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new DeletePhotoCommand { Id = id }, cancellationToken));
        }
    }
}
