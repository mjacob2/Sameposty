using FastEndpoints;
using FluentValidation;

namespace Sameposty.API.Endpoints.Posts.UpdatePostDescription;

public class UpdatePostDescriptionRequestValidator : Validator<UpdatePostDescriptionRequest>
{
    public UpdatePostDescriptionRequestValidator()
    {
        RuleFor(x => x.PostDescription)
            .NotNull()
            .WithMessage("Opis nie może być pusty")
            .NotEmpty()
            .WithMessage("Opis nie może być pusty")
            .MaximumLength(1000)
            .WithMessage("Opis nie więcej niż 1000 znaków");
    }
}
