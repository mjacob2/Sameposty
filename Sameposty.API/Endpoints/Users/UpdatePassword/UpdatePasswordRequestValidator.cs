using FastEndpoints;
using FluentValidation;

namespace Sameposty.API.Endpoints.Users.UpdatePassword;

public class UpdatePasswordRequestValidator : Validator<UpdatePasswordRequest>
{
    public UpdatePasswordRequestValidator()
    {
        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("Hasło nie może być puste")
            .NotEmpty()
            .WithMessage("Hasło nie może być puste")
            .MaximumLength(20)
            .WithMessage("Hasło nie więcej niż 50 znaków")
            .MinimumLength(8)
            .WithMessage("Hasło nie mniej niż 8 znaków");
    }
}
