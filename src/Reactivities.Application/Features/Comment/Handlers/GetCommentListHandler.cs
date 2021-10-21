using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Reactivities.Application.DTOs;
using Reactivities.Application.Features.Comment.Queries;
using Reactivities.Application.Helpers;
using Reactivities.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Application.Features.Comment.Handlers
{
    public class GetCommentListHandler : IRequestHandler<GetCommentListQuery, Result<List<CommentDto>>>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCommentListHandler> _logger;

        public GetCommentListHandler(ApplicationDbContext context,
            IMapper mapper,
            ILogger<GetCommentListHandler> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<List<CommentDto>>> Handle(GetCommentListQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetCommentListHandler.Handle - Retrieving comments.");

            var comments = await _context.Comments
                .Include(x => x.Activity)
                .Where(x => x.Activity.Id == request.ActivityId)
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();

            var commentsDto = _mapper.Map<List<CommentDto>>(comments);

            _logger.LogInformation("GetCommentListHandler.Handle - Retrieved comments successfully.");
            return Result<List<CommentDto>>.Success(commentsDto);
        }
    }
}
