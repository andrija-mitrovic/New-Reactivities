using MediatR;
using Reactivities.Domain.Entities;

namespace Reactivities.Application.Features.Activities.Commands
{
    public class EditActivityCommand : IRequest
    {
        public Activity Activity { get; set; }
    }
}
