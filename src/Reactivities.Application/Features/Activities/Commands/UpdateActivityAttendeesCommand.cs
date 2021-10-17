using MediatR;
using Reactivities.Application.Helpers;
using System;

namespace Reactivities.Application.Features.Activities.Commands
{
    public class UpdateActivityAttendeesCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; set; }
    }
}
