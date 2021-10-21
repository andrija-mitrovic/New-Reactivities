using MediatR;
using Reactivities.Application.DTOs;
using Reactivities.Application.Helpers;
using System;

namespace Reactivities.Application.Features.Comment.Commands
{
    public class CreateCommentCommand : IRequest<Result<CommentDto>>
    {
        public string Body { get; set; }
        public Guid ActivityId { get; set; }
    }
}
