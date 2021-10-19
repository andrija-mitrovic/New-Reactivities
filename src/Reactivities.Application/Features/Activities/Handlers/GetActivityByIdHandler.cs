using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.DTOs;
using Reactivities.Application.Features.Activities.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class GetActivityByIdHandler : IRequestHandler<GetActivityByIdQuery, Result<ActivityDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetActivityByIdHandler> _logger;

        public GetActivityByIdHandler(ApplicationDbContext context, 
            IMapper mapper,
            ILogger<GetActivityByIdHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ActivityDto>> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetActivityByIdHandler.Handle - Retrieving activity with id={request.Id}.");

            var activity = await _context.Activities
                .Include(x => x.ActivityAttendees)
                .ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            var activityDto = _mapper.Map<ActivityDto>(activity);

            _logger.LogInformation("GetActivityByIdHandler.Handle - Retrieved profile successfully");
            return Result<ActivityDto>.Success(activityDto);
        }
    }
}
