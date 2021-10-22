using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.DTOs;
using Reactivities.Application.Features.Followers.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
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
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<GetFollowersListCommand> _logger;

        public GetFollowersListCommand(ApplicationDbContext context, 
            IMapper mapper, 
            IUserAccessor userAccessor,
            ILogger<GetFollowersListCommand> logger)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<List<ProfileDto>>> Handle(GetFollowersListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetFollowersListCommand.Handle - Retrieving Followers.");

            var profilesDto = new List<ProfileDto>();

            switch (request.Predicate)
            {
                case "followers":
                    profilesDto = await _context.UserFollowings
                        .Where(x => x.Target.UserName == request.Username)
                        .Select(x => x.Observer)
                        .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider, 
                            new { currentUsername = _userAccessor.GetUsername() })
                        .ToListAsync(cancellationToken);

                    break;
                case "followering":
                    profilesDto = await _context.UserFollowings
                        .Where(x => x.Observer.UserName == request.Username)
                        .Select(x => x.Target)
                        .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider,
                            new { currentUsername = _userAccessor.GetUsername() })
                        .ToListAsync(cancellationToken);

                    break;
            }

            _logger.LogInformation("GetFollowersListCommand.Handle - Retrieved followers successfully");
            return Result<List<ProfileDto>>.Success(profilesDto);
        }
    }
}
