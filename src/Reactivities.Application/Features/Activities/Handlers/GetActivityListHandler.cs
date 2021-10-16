using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.DTOs;
using Reactivities.Application.Features.Activities.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class GetActivityListHandler : IRequestHandler<GetActivityListQuery, Result<List<ActivityDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetActivityListHandler> _logger;

        public GetActivityListHandler(ApplicationDbContext context,
            IMapper mapper,
            ILogger<GetActivityListHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<ActivityDto>>> Handle(GetActivityListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetActivityListHandler.Handle - Retrieving activities.");

            var activities = await _context.Activities
                .Include(x => x.ActivityAttendees)
                .ThenInclude(x => x.AppUser)
                .ToListAsync(cancellationToken);

            var activitiesDto = _mapper.Map<List<ActivityDto>>(activities);

            return Result<List<ActivityDto>>.Success(activitiesDto);
        }
    }
}
