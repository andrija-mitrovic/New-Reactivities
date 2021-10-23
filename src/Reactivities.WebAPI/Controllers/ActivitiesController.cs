using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Features.Activities.Commands;
using Reactivities.Application.Features.Activities.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.WebAPI.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetActivities([FromQuery] PagingParams param, CancellationToken cancellationToken)
        {
            return HandlePagedResult(await Mediator.Send(new GetActivityListQuery { Params = param }, cancellationToken));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(Guid id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new GetActivityByIdQuery() { Id = id }, cancellationToken));
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new CreateActivityCommand { Activity = activity }, cancellationToken));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity, CancellationToken cancellationToken)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new EditActivityCommand { Activity = activity }, cancellationToken));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new DeleteActivityCommand { Id = id }, cancellationToken));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new UpdateActivityAttendeesCommand { Id = id }, cancellationToken));
        }
    }
}
