using FastEndpoints;
using FluentValidation;

namespace Sameposty.API.Endpoints.Users.AddUser;

public class AddUserRequestValidator : Validator<AddUserRequest>
{
    public AddUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .WithMessage("Email nie może być pusty")
            .NotEmpty()
            .WithMessage("Email nie może być pusty")
            .MaximumLength(50)
            .WithMessage("Email nie więcej niż 50 znaków")
            .EmailAddress()
            .WithMessage("E-mail ma nieprawidłowy format");

        RuleFor(x => x.Password)
            .NotNull()
            .WithMessage("Hasło nie może być puste")
            .NotEmpty()
            .WithMessage("Hasło nie może być puste")
            .MaximumLength(20)
            .WithMessage("Hasło nie więcej niż 20 znaków")
            .MinimumLength(6)
            .WithMessage("Hasło nie mniej niż 6 znaków");

        RuleFor(x => x.NIP)
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
