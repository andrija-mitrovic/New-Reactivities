using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Photos.Commands;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
using Reactivities.Infrastructure.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Photos.Handlers
{
    public class SetMainPhotoHandler : IRequestHandler<SetMainPhotoCommand, Result<Unit>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<SetMainPhotoHandler> _logger;

        public SetMainPhotoHandler(ApplicationDbContext context,
            IUserAccessor userAccessor,
            ILogger<SetMainPhotoHandler> logger)
        {
            _context = context;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SetMainPhotoHandler.Handle - Setting main photo.");

            var user = await _context.Users.Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);

            if (user == null)
            {
                _logger.LogError("SetMainPhotoHandler.Handle - Current user couldn't be found.");
                return null;
            }

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

            if (photo == null)
            {
                _logger.LogError($"SetMainPhotoHandler.Handle - Photo couldn't be found with Id: {request.Id}.");
                return null;
            }

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

            if (currentMain != null) currentMain.IsMain = false;

            photo.IsMain = true;

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
            {
                _logger.LogInformation($"SetMainPhotoHandler.Handle - Successfully set main photo with Id: {request.Id}.");
                return Result<Unit>.Success(Unit.Value);
            }

            _logger.LogInformation($"Problem setting main photo with Id: {request.Id}.");
            return Result<Unit>.Failure("Problem setting main photo.");
        }
    }
}
