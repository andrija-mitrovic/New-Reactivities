using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.DTOs;
using Reactivities.Application.Features.Profiles.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Profiles.Handlers
{
    public class GetProfileByUsernameHandler : IRequestHandler<GetProfileByUsernameQuery, Result<ProfileDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetProfileByUsernameHandler> _logger;

        public GetProfileByUsernameHandler(ApplicationDbContext context,
            IMapper mapper,
            ILogger<GetProfileByUsernameHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ProfileDto>> Handle(GetProfileByUsernameQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetProfileByUsernameHandler.Handle - Retrieving profile with Username: {request.Username}");

            var user = await _context.Users
                .Include(x => x.ActivityAttendees)
                .ThenInclude(x => x.Activity)
                .FirstOrDefaultAsync(x => x.UserName == request.Username, cancellationToken);

            var userDto = _mapper.Map<ProfileDto>(user);

            _logger.LogInformation("GetProfileByUsernameHandler.Handle - Retrieved profile successfully.");
            return Result<ProfileDto>.Success(userDto);
        }
    }
}
