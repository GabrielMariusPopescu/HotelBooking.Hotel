namespace Hotel.Application.Validators;

public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
{
    public CreateHotelCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .MaximumLength(200)
            .WithMessage("Name can not be more than 200 characters.");
        
        RuleFor(command => command.Street)
            .NotEmpty()
            .MaximumLength(250)
            .WithMessage("Street can not be more than 250 characters.");
        
        RuleFor(command => command.City)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("City can not be more than 100 characters.");
        
        RuleFor(command => command.ZipCode)
            .NotEmpty()
            .MaximumLength(10)
            .WithMessage("Zip code can not be more than 10 characters.");
        
        RuleFor(command => command.Country)
            .NotEmpty()
            .MaximumLength(20)
            .WithMessage("Country can not be more than 20 characters.");
    }
}