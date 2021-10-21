using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.DTOs;
using Reactivities.Application.Features.Comment.Commands;
using Reactivities.Application.Helpers;
using Reactivities.Application.Interfaces;
using Reactivities.Infrastructure.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Comment.Handlers
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, Result<CommentDto>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger<CreateCommentHandler> _logger;

        public CreateCommentHandler(ApplicationDbContext context, 
            IMapper mapper, 
            IUserAccessor userAccessor,
            ILogger<CreateCommentHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("CreateCommentHandler.Handle - Creating comment.");

            var activity = await _context.Activities.FirstOrDefaultAsync(x => x.Id == request.ActivityId, cancellationToken);

            if (activity == null)
            {
                _logger.LogError($"CreateCommentHandler.Handle - Activity couldn't be found with Id: {request.ActivityId}");
                return null;
            }

            var user = await _context.Users
                .Include(x => x.Photos)
                .FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername(), cancellationToken);

            var comment = new Domain.Entities.Comment
            {
                Author = user,
                Activity = activity,
                Body = request.Body
            };

            activity.Comments.Add(comment);

            var success = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (success)
            {
                var commentDto = _mapper.Map<CommentDto>(comment);

                _logger.LogInformation("CreateCommentHandler.Handle - Comment successfully created.");
                return Result<CommentDto>.Success(commentDto);
            }

            _logger.LogError("CreateCommentHandler.Handle - Failed to add comment.");
            return Result<CommentDto>.Failure("Failed to add comment.");
        }
    }
}
