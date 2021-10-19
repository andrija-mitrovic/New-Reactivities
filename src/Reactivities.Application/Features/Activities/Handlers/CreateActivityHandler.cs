using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Activities.Commands;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
using Reactivities.Domain.Entities;
using Reactivities.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class CreateActivityHandler : IRequestHandler<CreateActivityCommand, Result<Unit>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<CreateActivityHandler> _logger;

        public CreateActivityHandler(ApplicationDbContext context,
            IUserAccessor userAccessor,
            ILogger<CreateActivityHandler> logger)
        {
            _context = context;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateActivityHandler.Handler - Creating activity.");

            var user = await _context.Users.FirstOrDefaultAsync(x => 
                x.UserName == _userAccessor.GetUsername(), cancellationToken);

            var activityAttendee = new ActivityAttendee
            {
                AppUser = user,
                Activity = request.Activity,
                IsHost = true
            };

            request.Activity.ActivityAttendees.Add(activityAttendee);

            _context.Activities.Add(request.Activity);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) 
            {
                _logger.LogError("CreateActivityHandler.Handler - Failed to create activity");
                return Result<Unit>.Failure("Failed to create activity");
            }

            _logger.LogInformation("CreateActivityHandler.Handler - Successfully created activity");
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
