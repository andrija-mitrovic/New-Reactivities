using MediatR;
using Reactivities.Domain.Entities;
using System;

namespace Reactivities.Application.Features.Activities.Queries
{
    public class GetActivityByIdQuery : IRequest<Activity>
    {
        public Guid Id { get; set; }
    }
}
