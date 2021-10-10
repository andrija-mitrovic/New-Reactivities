using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Activities.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Domain.Entities;
using Reactivities.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class GetActivityListHandler : IRequestHandler<GetActivityListQuery, Result<List<Activity>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetActivityListHandler> _logger;

        public GetActivityListHandler(ApplicationDbContext context,
            ILogger<GetActivityListHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<List<Activity>>> Handle(GetActivityListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetActivityListHandler.Handle - Retrieving activities.");

            return Result<List<Activity>>.Success(await _context.Activities.ToListAsync(cancellationToken));
        }
    }
}
