using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Activities.Commands;
using Reactivities.Application.Helpers;
using Reactivities.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class EditActivityHandler : IRequestHandler<EditActivityCommand, Result<Unit>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditActivityHandler> _logger;

        public EditActivityHandler(ApplicationDbContext context,
            ILogger<EditActivityHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(EditActivityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"EditActivityHandler.Handle - Editing activity with id={request.Activity.Id}.");

            var activity = await _context.Activities
                .FirstOrDefaultAsync(x => x.Id == request.Activity.Id, cancellationToken);

            if (activity == null) return null;

            activity.Id = request.Activity.Id;

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                _logger.LogInformation("EditActivityHandler.Handle - Failed to update activity");
                return Result<Unit>.Failure("Failed to update activity");
            }

            _logger.LogInformation("EditActivityHandler.Handle - Successfully edited activity");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
