using MediatR;
using Reactivities.Application.DTOs;
using Reactivities.Application.Helpers;
using Reactivities.Domain.Entities;
using System;

namespace Reactivities.Application.Features.Activities.Queries
{
    public class GetActivityByIdQuery : IRequest<Result<ActivityDto>>
    {
        public Guid Id { get; set; }
    }
}
