using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.DTOs;
using Reactivities.Application.Features.Followers.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Domain.Entities;
using Reactivities.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Followers.Handlers
{
    public class GetFollowersListCommand : IRequestHandler<GetFollowersListQuery, Result<List<ProfileDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetFollowersListCommand> _logger;

        public GetFollowersListCommand(ApplicationDbContext context, 
            IMapper mapper, 
            ILogger<GetFollowersListCommand> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<ProfileDto>>> Handle(GetFollowersListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetFollowersListCommand.Handle - Retrieving Followers.");

            var profilesDto = new List<ProfileDto>();

            List<AppUser> profiles = null;

            switch (request.Predicate)
            {
                case "followers":
                    profiles = await _context.UserFollowings.Include(x => x.Observer)
                        .Where(x => x.Target.UserName == request.Username)
                        .Select(x => x.Observer)
                        .ToListAsync(cancellationToken);

                    profilesDto = _mapper.Map<List<ProfileDto>>(profiles);

                    break;
                case "followering":
                    profiles = await _context.UserFollowings.Include(x => x.Target)
                        .Where(x => x.Observer.UserName == request.Username)
                        .Select(x => x.Target)
                        .ToListAsync(cancellationToken);

                    profilesDto = _mapper.Map<List<ProfileDto>>(profiles);

                    break;
            }

            _logger.LogInformation("GetFollowersListCommand.Handle - Retrieved followers successfully");
            return Result<List<ProfileDto>>.Success(profilesDto);
        }
    }
}
