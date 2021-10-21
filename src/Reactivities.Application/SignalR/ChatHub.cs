using MediatR;
using Microsoft.AspNetCore.SignalR;
using Reactivities.Application.Features.Comment.Commands;
using Reactivities.Application.Features.Comment.Queries;
using System;
using System.Threading.Tasks;

namespace Reactivities.Application.SignalR
{
    public class ChatHub : Hub
    {
        private readonly IMediator _mediator;

        public ChatHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task SendComment(CreateCommentCommand createCommentCommand)
        {
            var comment = await _mediator.Send(createCommentCommand);

            await Clients.Group(createCommentCommand.ActivityId.ToString())
                .SendAsync("ReceiveComment", comment.Value);
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var activityId = httpContext.Request.Query["activityId"];
            await Groups.AddToGroupAsync(Context.ConnectionId, activityId);
            var result = await _mediator.Send(new GetCommentListQuery { ActivityId = Guid.Parse(activityId) });
            await Clients.Caller.SendAsync("LoadComments", result.Value);
        }
    }
}
