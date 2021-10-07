using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Activities.Commands;
using Reactivities.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class CreateActivityHandler : IRequestHandler<CreateActivityCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CreateActivityHandler> _logger;

        public CreateActivityHandler(ApplicationDbContext context,
            ILogger<CreateActivityHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateActivityHandler.Handler - Creating activity.");

            await _context.Activities.AddAsync(request.Activity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
