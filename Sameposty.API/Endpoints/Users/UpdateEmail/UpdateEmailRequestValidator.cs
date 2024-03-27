using FastEndpoints;
using FluentValidation;

namespace Sameposty.API.Endpoints.Users.UpdateEmail;

public class UpdateEmailRequestValidator : Validator<UpdateEmailRequest>
{
    public UpdateEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage("E-mail nie może być pusty")
            .NotEmpty()
            .WithMessage("E-mail nie może być pusty")
            .MaximumLength(50)
            .WithMessage("E-mail nie więcej niż 50 znaków")
            .EmailAddress()
            .WithMessage("E-mail ma nieprawidłowy format");
    }
}
