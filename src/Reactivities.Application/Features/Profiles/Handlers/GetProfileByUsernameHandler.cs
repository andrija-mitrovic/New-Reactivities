using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.DTOs;
using Reactivities.Application.Features.Profiles.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
using Reactivities.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Profiles.Handlers
{
    public class GetProfileByUsernameHandler : IRequestHandler<GetProfileByUsernameQuery, Result<ProfileDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<GetProfileByUsernameHandler> _logger;

        public GetProfileByUsernameHandler(ApplicationDbContext context,
            IMapper mapper,
            IUserAccessor userAccessor,
            ILogger<GetProfileByUsernameHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<ProfileDto>> Handle(GetProfileByUsernameQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetProfileByUsernameHandler.Handle - Retrieving profile with Username: {request.Username}");

            var userDto = await _context.Users
                .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider,
                    new { currentUsername = _userAccessor.GetUsername() })
                .FirstOrDefaultAsync(x => x.Username == request.Username, cancellationToken);

            _logger.LogInformation("GetProfileByUsernameHandler.Handle - Retrieved profile successfully.");
            return Result<ProfileDto>.Success(userDto);
        }
    }
}
