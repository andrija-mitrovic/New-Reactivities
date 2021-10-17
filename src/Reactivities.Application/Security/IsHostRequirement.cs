using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Reactivities.Infrastructure.Data;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reactivities.Application.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {

    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsHostRequirementHandler(ApplicationDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Task.CompletedTask;

            var activityId = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues
                .SingleOrDefault(x => x.Key == "id").Value?.ToString());

            var activityAttendee = _dbContext.ActivityAttendees.FindAsync(userId, activityId).Result;

            if (activityAttendee == null) return Task.CompletedTask;

            if (activityAttendee.IsHost) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
