using ErrorOr;
using MediatR;
using Products.Application.Common.Abstractions;
using Products.Domain.Products;

namespace Products.Application.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<UpdateProductCommand, ErrorOr<Product>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<ErrorOr<Product>> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_productRepository.GetById(command.ProductId) is not Product product)
        {
            return Error.NotFound("ProductNotFound", "Product not found");
        }

        product.Update(command.Name, command.Description, command.Price);
        _productRepository.Update(product);

        return product;
    }
}