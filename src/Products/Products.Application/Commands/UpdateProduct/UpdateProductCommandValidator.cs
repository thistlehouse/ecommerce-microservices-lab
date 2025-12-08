using FluentValidation;

namespace Products.Application.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.ProductId)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product id cannot be null or empty")
            .NotEqual(Guid.Empty)
            .WithMessage("Product id cannot have default value");

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