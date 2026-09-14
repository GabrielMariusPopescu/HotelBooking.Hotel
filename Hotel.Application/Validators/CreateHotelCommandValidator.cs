namespace Hotel.Application.Validators;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(it => it.Name).NotEmpty().MaximumLength(200);
        RuleFor(it => it.Street).NotEmpty().MaximumLength(250);
        RuleFor(it => it.City).NotEmpty().MaximumLength(100);
        RuleFor(it => it.ZipCode).NotEmpty().MaximumLength(10);

        RuleFor(it => it.CountryCode)
            .NotEmpty()
            .Length(2)
            .WithMessage("CountryCode must be exact 2 characters.");
    }
}