using FluentValidation;
using Reactivities.Application.Features.Activities.Commands;

namespace Reactivities.Application.Features.Activities.Validators
{
    public class CreateActivityCommandValidator : AbstractValidator<CreateActivityCommand>
    {
        public CreateActivityCommandValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
        }
    }
}
