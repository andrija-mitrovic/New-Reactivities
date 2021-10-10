using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Activities.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Domain.Entities;
using Reactivities.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class GetActivityByIdHandler : IRequestHandler<GetActivityByIdQuery, Result<Activity>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GetActivityByIdHandler> _logger;

        public GetActivityByIdHandler(ApplicationDbContext context, 
            ILogger<GetActivityByIdHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Activity>> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"GetActivityByIdHandler.Handle - Retrieving activity with id={request.Id}.");

            return Result<Activity>.Success(await _context.Activities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken));
        }
    }
}
