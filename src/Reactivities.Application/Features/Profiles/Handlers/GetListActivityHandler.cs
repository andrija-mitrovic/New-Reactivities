using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.DTOs;
using Reactivities.Application.Features.Profiles.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Profiles.Handlers
{
    public class GetListActivityHandler : IRequestHandler<GetListActivityQuery, Result<List<UserActivityDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetListActivityHandler> _logger;

        public GetListActivityHandler(ApplicationDbContext context,
            IMapper mapper,
            ILogger<GetListActivityHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<UserActivityDto>>> Handle(GetListActivityQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetListActivityHandler.Handle - Retreiving list of user activity.");

            var query = _context.ActivityAttendees
                .Where(x => x.AppUser.UserName == request.Username)
                .OrderBy(x => x.Activity.Date)
                .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            query = request.Predicate switch
            {
                "post" => query.Where(x => x.Date <= DateTime.Now),
                "hosting" => query.Where(x => x.HostUsername == request.Username),
                _ => query.Where(x => x.Date >= DateTime.Now)
            };

            var activities = await query.ToListAsync();

            _logger.LogInformation("GetListActivityHandler.Handle - Successfully retrieved user activity.");
            return Result<List<UserActivityDto>>.Success(activities);
        }
    }
}
