using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.Features.Photos.Commands;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
using Reactivities.Domain.Entities;
using Reactivities.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Photos.Handlers
{
    public class CreatePhotoHandler : IRequestHandler<CreatePhotoCommand, Result<Photo>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<CreatePhotoHandler> _logger;

        public CreatePhotoHandler(ApplicationDbContext context,
            IPhotoService photoService,
            IUserAccessor userAccessor,
            ILogger<CreatePhotoHandler> logger)
        {
            _context = context;
            _photoService = photoService;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<Photo>> Handle(CreatePhotoCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreatePhotoHandler.Handle - Creating photo.");

            var user = await _context.Users.Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);

            if (user == null) 
            {
                _logger.LogError("CreatePhotoHandler.Handle - Current user couldn't be found");
                return null; 
            }

            var photoUploadResult = await _photoService.AddPhoto(request.File);

            if (photoUploadResult != null)
            {
                var photo = new Photo
                {
                    Url = photoUploadResult.Url,
                    Id = photoUploadResult.PublicId
                };

                if (!user.Photos.Any(x => x.IsMain)) photo.IsMain = true;

                user.Photos.Add(photo);

                var result = await _context.SaveChangesAsync(cancellationToken) > 0;

                if (result)
                {
                    _logger.LogInformation("CreatePhotoHandler.Handle - Photo created successfully.");
                    return Result<Photo>.Success(photo);
                }
            }

            _logger.LogError("CreatePhotoHandler.Handle - Problem adding photo.");
            return Result<Photo>.Failure("Problem adding photo");
        }
    }
}
