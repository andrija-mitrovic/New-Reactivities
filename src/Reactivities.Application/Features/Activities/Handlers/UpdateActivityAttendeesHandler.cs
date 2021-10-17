using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Activities.Commands;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
using Reactivities.Domain.Entities;
using Reactivities.Infrastructure.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Activities.Handlers
{
    public class UpdateActivityAttendeesHandler : IRequestHandler<UpdateActivityAttendeesCommand, Result<Unit>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<UpdateActivityAttendeesHandler> _logger;

        public UpdateActivityAttendeesHandler(ApplicationDbContext context,
            IUserAccessor userAccessor,
            ILogger<UpdateActivityAttendeesHandler> logger)
        {
            _context = context;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(UpdateActivityAttendeesCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"UpdateActivityAttendeesHandler.Handle - Updating ActivityAttendees with id={request.Id}.");

            var activity = await _context.Activities
                .Include(x => x.ActivityAttendees)
                .ThenInclude(x => x.AppUser)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (activity == null) return null;

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

            if (user == null) return null;

            var hostUsername = activity.ActivityAttendees.FirstOrDefault(x => x.IsHost)?.AppUser?.UserName;

            var activityAttendee = activity.ActivityAttendees.FirstOrDefault(x => x.AppUser.UserName == user.UserName);

            if (activityAttendee != null && hostUsername == user.UserName)
                activity.IsCancelled = !activity.IsCancelled;

            if (activity != null && hostUsername != user.UserName)
                activity.ActivityAttendees.Remove(activityAttendee);

            if (activityAttendee == null)
            {
                activityAttendee = new ActivityAttendee
                {
                    AppUser = user,
                    Activity = activity,
                    IsHost = false
                };

                activity.ActivityAttendees.Add(activityAttendee);
            }

            var result = await _context.SaveChangesAsync() > 0;

            if (result)
            {
                _logger.LogInformation("UpdateActivityAttendeesHandler.Handle - Successfully updated ActivityAttendee");
                return Result<Unit>.Success(Unit.Value);
            }
            else 
            {
                _logger.LogInformation("UpdateActivityAttendeesHandler.Handle - Problem updating ActivityAttendee");
                return Result<Unit>.Failure("Problem updating ActivityAttendee"); 
            }
        }
    }
}
