using MediatR;
using Products.Application.Abstractions.Repositories;
using Products.Domain.Products;

namespace Products.Application.Commands.CreateProducts;

public sealed class CreateProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<CreateProductResponse> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        Product product = Product.Create(
            command.Name,
            command.Description,
            command.Price);

        _productRepository.Add(product);

        return new CreateProductResponse(product.Id);
    }
}
