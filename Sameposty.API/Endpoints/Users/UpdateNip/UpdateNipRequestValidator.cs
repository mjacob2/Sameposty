using FastEndpoints;
using FluentValidation;

namespace Sameposty.API.Endpoints.Users.UpdateNip;

public class UpdateNipRequestValidator : Validator<UpdateNipRequest>
{
    public UpdateNipRequestValidator()
    {
        RuleFor(x => x.Nip)
            .NotNull()
            .WithMessage("NIP nie może być pusty")
            .NotEmpty()
            .WithMessage("NIP nie może być pusty")
            .MaximumLength(10)
            .WithMessage("NIP nie więcej niż 10 znaków")
            .MinimumLength(10)
            .WithMessage("NIP nie mniej niż 10 znaków")
            .Matches(@"^\d+$")
            .WithMessage("NIP może zawierać tylko cyfry");
    }
}
