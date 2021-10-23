using MediatR;
using Reactivities.Application.DTOs;
using Reactivities.Application.Helpers;
using System.Collections.Generic;

namespace Reactivities.Application.Features.Profiles.Queries
{
    public class GetListActivityQuery : IRequest<Result<List<UserActivityDto>>>
    {
        public string Username { get; set; }
        public string Predicate { get; set; }
    }
}
