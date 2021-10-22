using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Followers.Commands;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
using Reactivities.Domain.Entities;
using Reactivities.Infrastructure.Data;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Followers.Handlers
{
    public class FollowToggleHandler : IRequestHandler<FollowToggleCommand, Result<Unit>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<FollowToggleHandler> _logger;

        public FollowToggleHandler(ApplicationDbContext context, 
            IUserAccessor userAccessor, 
            ILogger<FollowToggleHandler> logger)
        {
            _context = context;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(FollowToggleCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("FollowToggleHandler.Handle - Toggling followings.");

            var observer = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);

            var target = await _context.Users
                .FirstOrDefaultAsync(x => x.UserName == request.TargetUsername, cancellationToken);

            if (target == null)
            {
                _logger.LogError($"FollowToggleHandler.Handle - Target user couldn't be found with Username: {request.TargetUsername}");
                return null;
            }

            var following = await _context.UserFollowings.FindAsync(observer.Id, target.Id);

            if (following == null)
            {
                following = new UserFollowing
                {
                    Observer = observer,
                    Target = target
                };

                _context.UserFollowings.Add(following);
            }
            else
            {
                _context.UserFollowings.Remove(following);
            }

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
            {
                _logger.LogInformation("FollowToggleHandler.Handle - Successfully update following.");
                return Result<Unit>.Success(Unit.Value);
            }

            _logger.LogError("FollowToggleHandler.Handle - Failed to update following.");
            return Result<Unit>.Failure("Failed to update following");
        }
    }
}
