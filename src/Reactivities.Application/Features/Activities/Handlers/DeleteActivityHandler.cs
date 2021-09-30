using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Activities.Commands;
using Reactivities.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class DeleteActivityHandler : IRequestHandler<DeleteActivityCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DeleteActivityCommand> _logger;

        public DeleteActivityHandler(ApplicationDbContext context,
            ILogger<DeleteActivityCommand> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"DeleteActivityHandler.Handle - Deleteing activity with id={request.Id}.");

            var activity = await _context.Activities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            _context.Activities.Remove(activity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
