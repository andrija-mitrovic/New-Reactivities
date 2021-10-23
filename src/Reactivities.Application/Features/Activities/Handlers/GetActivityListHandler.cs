using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Application.DTOs;
using Reactivities.Application.Features.Activities.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
using Reactivities.Infrastructure.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class GetActivityListHandler : IRequestHandler<GetActivityListQuery, Result<PagedList<ActivityDto>>>
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

        public async Task<Result<PagedList<ActivityDto>>> Handle(GetActivityListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetActivityListHandler.Handle - Retrieving activities.");

            var query = _context.Activities
                .OrderBy(x => x.Date)
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider,
                    new { currentUsername = _userAccessor.GetUsername() })
                .AsQueryable();

            _logger.LogInformation("GetActivityListHandler.Handle - Retrieved profiles successfully.");
            return Result<PagedList<ActivityDto>>.Success(
                await PagedList<ActivityDto>.CreateAsync(query, request.Params.PageNumber, 
                    request.Params.PageSize, cancellationToken));
        }
    }
}
