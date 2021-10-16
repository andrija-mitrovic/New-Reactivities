using MediatR;
using Reactivities.Application.DTOs;
using Reactivities.Application.Helpers;
using System.Collections.Generic;

namespace Reactivities.Application.Features.Activities.Queries
{
    public class GetActivityListQuery : IRequest<Result<List<ActivityDto>>>
    {
    }
}
