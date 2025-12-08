using FluentValidation;

namespace Products.Application.Queries.GetProducts;

public sealed class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
{
    public GetProductsQueryValidator()
    {
        RuleFor(q => q.Page)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");

        RuleFor(q => q.PageSize)
            .GreaterThan(0)
            .WithMessage("Page must be greater than 0");
    }
}