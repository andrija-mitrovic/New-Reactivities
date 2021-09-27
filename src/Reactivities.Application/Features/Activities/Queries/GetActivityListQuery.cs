using MediatR;
using Reactivities.Domain.Entities;
using System.Collections.Generic;

namespace Reactivities.Application.Features.Activities.Queries
{
    public class GetActivityListQuery : IRequest<List<Activity>>
    {
    }
}
