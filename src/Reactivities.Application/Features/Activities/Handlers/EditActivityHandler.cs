using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly ILogger<EditActivityHandler> _logger;

        public EditActivityHandler(ApplicationDbContext context,
            IMapper mapper,
            ILogger<EditActivityHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(EditActivityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"EditActivityHandler.Handle - Editing activity with id={request.Activity.Id}.");

            var activity = await _context.Activities
                .FirstOrDefaultAsync(x => x.Id == request.Activity.Id, cancellationToken);

            if (activity == null)
            {
                _logger.LogError($"EditActivityHandler.Handle - Activity couldn't be found.");
                return null;
            }

            _mapper.Map(request.Activity, activity);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result)
            {
                _logger.LogError("EditActivityHandler.Handle - Failed to update activity");
                return Result<Unit>.Failure("Failed to update activity");
            }

            _logger.LogInformation("EditActivityHandler.Handle - Successfully edited activity");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
