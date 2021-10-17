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
    public class DeleteActivityHandler : IRequestHandler<DeleteActivityCommand, Result<Unit>>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteActivityCommand> _logger;

        public DeleteActivityHandler(ApplicationDbContext context,
            ILogger<DeleteActivityCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteActivityHandler.Handle - Deleteing activity with id={request.Id}.");

            var activity = await _context.Activities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (activity == null) return null;

            _context.Activities.Remove(activity);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete activity");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}
