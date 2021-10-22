using MediatR;
using Reactivities.Application.DTOs;
using Reactivities.Application.Helpers;
using System.Collections.Generic;

namespace Reactivities.Application.Features.Followers.Queries
{
    public class GetFollowersListQuery : IRequest<Result<List<ProfileDto>>>
    {
        public string Predicate { get; set; }
        public string Username { get; set; }
    }
}
