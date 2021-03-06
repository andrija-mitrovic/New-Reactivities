using MediatR;
using Reactivities.Application.Helpers;
using Reactivities.Domain.Entities;

namespace Reactivities.Application.Features.Activities.Commands
{
    public class EditActivityCommand : IRequest<Result<Unit>>
    {
        public Activity Activity { get; set; }
    }
}
