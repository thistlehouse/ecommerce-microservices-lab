using FluentValidation;
using Inventories.Application.Commands.CreateStockItem;

namespace Inventories.Application.Commands.CreateItemStock;

public class CreateStockItemCommandValidator : AbstractValidator<CreateStockItemCommand>
{
    public CreateStockItemCommandValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty()
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .WithMessage("Name must be between 3 and 50 characters");
    }
}