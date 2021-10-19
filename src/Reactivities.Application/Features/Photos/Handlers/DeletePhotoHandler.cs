using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Photos.Commands;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
using Reactivities.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Photos.Handlers
{
    public class DeletePhotoHandler : IRequestHandler<DeletePhotoCommand, Result<Unit>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<DeletePhotoHandler> _logger;

        public DeletePhotoHandler(ApplicationDbContext context,
            IPhotoService photoService,
            IUserAccessor userAccessor,
            ILogger<DeletePhotoHandler> logger)
        {
            _context = context;
            _photoService = photoService;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("DeletePhotoHandler.Handle - Deleting photo.");

            var user = await _context.Users.Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);

            if (user == null)
            {
                _logger.LogError("DeletePhotoHandler.Handle - Current user couldn't be found.");
                return null;
            }

            var photo = user.Photos.FirstOrDefault(x => x.Id == request.Id);

            if (photo == null)
            {
                _logger.LogError($"DeletePhotoHandler.Handle - Photo couldn't be found with Id: {request.Id}");
                return null;
            }

            if (photo.IsMain)
            {
                _logger.LogError("DeletePhotoHandler.Handle - You cannot delete your main photo.");
                return Result<Unit>.Failure("You cannot delete your main photo.");
            }

            var result = await _photoService.DeletePhoto(photo.Id);

            if (result == null)
            {
                _logger.LogError($"DeletePhotoHandler.Handle - Problem deleting photo from Cloudinary with Id: {request.Id}.");
                return Result<Unit>.Failure("Problem deleting photo from Cloudinary");
            }

            user.Photos.Remove(photo);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
            {
                _logger.LogInformation($"DeletePhotoHandler.Handle - Successfully deleted photo with Id: {request.Id}.");
                return Result<Unit>.Success(Unit.Value);
            }

            _logger.LogInformation($"Problem deleting photo from API with Id: {request.Id}.");
            return Result<Unit>.Failure("Problem deleting photo from API");
        }
    }
}
