using FastEndpoints;
using FluentValidation;

namespace Sameposty.API.Endpoints.BasicInformations.UpdateById;

public class UpdateBasicInformationByIdRequestValidator : Validator<UpdateBasicInformationByIdRequest>
{
    public UpdateBasicInformationByIdRequestValidator()
    {
        RuleFor(x => x.Branch)
            .NotNull()
            .WithMessage("Branża nie może być pusta")
            .NotEmpty()
            .WithMessage("Branża nie może być pusty")
            .MaximumLength(200)
            .WithMessage("Branża nie więcej niż 200 znaków");

        RuleFor(x => x.ProductsAndServices)
            .NotNull()
            .WithMessage("Produkty/usługi nie może być pusta")
            .NotEmpty()
            .WithMessage("Produkty/usługi nie może być pusty")
            .MaximumLength(500)
            .WithMessage("Produkty/usługi nie więcej niż 500 znaków");

        RuleFor(x => x.Goals)
            .NotNull()
            .WithMessage("Cel nie może być pusty")
            .NotEmpty()
            .WithMessage("Cel nie może być pusty")
            .MaximumLength(500)
            .WithMessage("Cel nie więcej niż 500 znaków");

        RuleFor(x => x.Assets)
            .NotNull()
            .WithMessage("Atut nie może być pusty")
            .NotEmpty()
            .WithMessage("Atut nie może być pusty")
            .MaximumLength(500)
            .WithMessage("CAtutel nie więcej niż 500 znaków");
    }
}
