using FluentValidation;
using Reactivities.Application.Features.Comment.Commands;

namespace Reactivities.Application.Features.Comment.Validators
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator()
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}
