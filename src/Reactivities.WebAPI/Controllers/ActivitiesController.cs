using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Features.Activities.Queries;
using Reactivities.Domain.Entities;
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
    }
}
