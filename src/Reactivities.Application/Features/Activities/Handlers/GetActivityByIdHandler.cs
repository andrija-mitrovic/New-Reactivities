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
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class GetActivityByIdHandler : IRequestHandler<GetActivityByIdQuery, Result<ActivityDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<GetActivityByIdHandler> _logger;

        public GetActivityByIdHandler(ApplicationDbContext context, 
            IMapper mapper,
            IUserAccessor userAccessor,
            ILogger<GetActivityByIdHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<ActivityDto>> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetActivityByIdHandler.Handle - Retrieving activity with id={request.Id}.");

            var activityDto = await _context.Activities
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                    new { currentUsername = _userAccessor.GetUsername() })
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            _logger.LogInformation("GetActivityByIdHandler.Handle - Retrieved profile successfully");
            return Result<ActivityDto>.Success(activityDto);
        }
    }
}
