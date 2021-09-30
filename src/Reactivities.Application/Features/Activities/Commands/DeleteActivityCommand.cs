using MediatR;
using System;

namespace Reactivities.Application.Features.Activities.Commands
{
    public class DeleteActivityCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
