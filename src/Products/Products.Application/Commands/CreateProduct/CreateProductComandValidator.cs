using FluentValidation;
using Products.Domain.Products;

namespace Products.Application.Commands.CreateProducts;

public class CreateProductValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .WithMessage("Name must be between 3 and 50 characters");

        RuleFor(p => p.Name)
            .NotEmpty()
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Description must have at least 3 characters");

        RuleFor(p => p.Price)
            .GreaterThan(0)
            .WithMessage("Price must have a value greater then 0");
    }
}