using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Activities.Commands;
using Reactivities.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class EditActivityHandler : IRequestHandler<EditActivityCommand>
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EditActivityHandler> _logger;

        public EditActivityHandler(ApplicationDbContext context,
            ILogger<EditActivityHandler> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Unit> Handle(EditActivityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"EditActivityHandler.Handle - Editing activity with id={request.Activity.Id}.");

            var activity = await _context.Activities
                .FirstOrDefaultAsync(x => x.Id == request.Activity.Id, cancellationToken);

            activity.Id = request.Activity.Id;

            await _context.SaveChangesAsync();

            return Unit.Value;
        }
    }
}
