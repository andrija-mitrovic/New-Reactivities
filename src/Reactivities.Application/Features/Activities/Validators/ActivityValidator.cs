using FluentValidation;
using Reactivities.Domain.Entities;

namespace Reactivities.Application.Features.Activities.Validators
{
    public class ActivityValidator : AbstractValidator<Activity>
    {
        public ActivityValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Venue).NotEmpty();
        }
    }
}
