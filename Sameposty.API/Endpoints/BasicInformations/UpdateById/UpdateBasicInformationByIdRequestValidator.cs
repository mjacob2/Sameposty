using FastEndpoints;
using FluentValidation;

namespace Sameposty.API.Endpoints.BasicInformations.UpdateById;

public class UpdateBasicInformationByIdRequestValidator : Validator<UpdateBasicInformationByIdRequest>
{
    public UpdateBasicInformationByIdRequestValidator()
    {
        RuleFor(x => x.BrandName)
            .NotNull()
            .WithMessage("Nazwa firmy nie może być pusta")
            .NotEmpty()
            .WithMessage("Nazwa firmy nie może być pusta")
            .MaximumLength(1000)
            .WithMessage("Nazwa firmy nie więcej niż 1000 znaków");

        RuleFor(x => x.Audience)
            .NotNull()
            .WithMessage("Klienci firmy nie może być puste")
            .NotEmpty()
            .WithMessage("Klienci firmy nie może być pusty")
            .MaximumLength(1000)
            .WithMessage("Klienci firmy nie więcej niż 1000 znaków");

        RuleFor(x => x.Mission)
            .NotNull()
            .WithMessage("Misja firmy nie może być pusta")
            .NotEmpty()
            .WithMessage("Misja firmy nie może być pusty")
            .MaximumLength(1000)
            .WithMessage("Misja firmy nie więcej niż 1000 znaków");

        RuleFor(x => x.ProductsAndServices)
            .NotNull()
            .WithMessage("Produkty/usługi nie może być puste")
            .NotEmpty()
            .WithMessage("Produkty/usługi nie może być pusty")
            .MaximumLength(1000)
            .WithMessage("Produkty/usługi nie więcej niż 1000 znaków");

        RuleFor(x => x.Goals)
            .NotNull()
            .WithMessage("Cel nie może być pusty")
            .NotEmpty()
            .WithMessage("Cel nie może być pusty")
            .MaximumLength(1000)
            .WithMessage("Cel nie więcej niż 1000 znaków");

        RuleFor(x => x.Assets)
            .NotNull()
            .WithMessage("Atut nie może być pusty")
            .NotEmpty()
            .WithMessage("Atut nie może być pusty")
            .MaximumLength(1000)
            .WithMessage("CAtutel nie więcej niż 1000 znaków");
    }
}
