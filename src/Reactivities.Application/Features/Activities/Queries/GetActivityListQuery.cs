using MediatR;
using Reactivities.Application.DTOs;
using Reactivities.Application.Helpers;

namespace Reactivities.Application.Features.Activities.Queries
{
    public class GetActivityListQuery : IRequest<Result<PagedList<ActivityDto>>>
    {
        public PagingParams Params { get; set; }
    }
}
