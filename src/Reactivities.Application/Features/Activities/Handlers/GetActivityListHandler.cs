using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.DTOs;
using Reactivities.Application.Features.Activities.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
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
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<GetActivityListHandler> _logger;

        public GetActivityListHandler(ApplicationDbContext context,
            IMapper mapper,
            IUserAccessor userAccessor,
            ILogger<GetActivityListHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<List<ActivityDto>>> Handle(GetActivityListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetActivityListHandler.Handle - Retrieving activities.");

            var activitiesDto = await _context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                    new { currentUsername = _userAccessor.GetUsername() })
                .ToListAsync(cancellationToken);

            _logger.LogInformation("GetActivityListHandler.Handle - Retrieved profiles successfully.");
            return Result<List<ActivityDto>>.Success(activitiesDto);
        }
    }
}
