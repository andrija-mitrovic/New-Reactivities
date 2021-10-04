using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Features.Activities.Commands;
using Reactivities.Application.Features.Activities.Queries;
using Reactivities.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.WebAPI.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        [HttpGet]
        public async Task<List<Activity>> GetActivities(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetActivityListQuery(), cancellationToken);
        }

        [HttpGet("{id}")]
        public async Task<Activity> GetActivity(Guid id, CancellationToken cancellationToken)
        {
            return await Mediator.Send(new GetActivityByIdQuery() { Id = id }, cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new CreateActivityCommand { Activity = activity }, cancellationToken));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity, CancellationToken cancellationToken)
        {
            activity.Id = id;
            return Ok(await Mediator.Send(new EditActivityCommand { Activity = activity }, cancellationToken));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new DeleteActivityCommand { Id = id }, cancellationToken));
        }
    }
}
