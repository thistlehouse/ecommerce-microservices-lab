using ErrorOr;
using MediatR;
using Products.Application.Common.Abstractions;
using Products.Contracts.Products;
using Products.Domain.Products;

namespace Products.Application.Commands.CreateProducts;

public sealed class CreateProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<CreateProductCommand, ErrorOr<CreateProductResult>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<ErrorOr<CreateProductResult>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        Product product = Product.Create(
            command.Name,
            command.Description,
            command.Price);

        _productRepository.Add(product);

        return new CreateProductResult(product.Id);
    }
}
