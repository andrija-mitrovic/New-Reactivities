using MediatR;
using Reactivities.Application.Helpers;
using Reactivities.Domain.Entities;
using System;

namespace Reactivities.Application.Features.Activities.Queries
{
    public class GetActivityByIdQuery : IRequest<Result<Activity>>
    {
        public Guid Id { get; set; }
    }
}
