using MediatR;
using Reactivities.Application.DTOs;
using Reactivities.Application.Helpers;

namespace Reactivities.Application.Features.Profiles.Queries
{
    public class GetProfileByUsernameQuery : IRequest<Result<ProfileDto>>
    {
        public string Username { get; set; }
    }
}
