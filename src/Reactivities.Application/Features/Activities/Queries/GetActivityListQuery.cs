using MediatR;
using Reactivities.Application.Helpers;
using Reactivities.Domain.Entities;
using System.Collections.Generic;

namespace Reactivities.Application.Features.Activities.Queries
{
    public class GetActivityListQuery : IRequest<Result<List<Activity>>>
    {
    }
}
