using FastEndpoints;
using FluentValidation;

namespace Sameposty.API.Endpoints.Users.ResetPassword;

public class ResetPasswordRequestValidator : Validator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage("Email nie może być pusty")
            .NotEmpty()
            .WithMessage("Email nie może być pusty")
            .MaximumLength(50)
            .WithMessage("Email nie więcej niż 50 znaków");
    }
}
