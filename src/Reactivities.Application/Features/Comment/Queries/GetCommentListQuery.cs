using MediatR;
using Reactivities.Application.DTOs;
using Reactivities.Application.Helpers;
using System;
using System.Collections.Generic;

namespace Reactivities.Application.Features.Comment.Queries
{
    public class GetCommentListQuery : IRequest<Result<List<CommentDto>>>
    {
        public Guid ActivityId { get; set; }
    }
}
