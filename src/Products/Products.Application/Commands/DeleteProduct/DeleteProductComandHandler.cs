using ErrorOr;
using MediatR;
using Products.Application.Common.Abstractions;
using Products.Domain.Products;

namespace Products.Application.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<DeleteProductCommand, ErrorOr<DeleteProductResult>>
{
    private readonly IProductRepository _productRepository = productRepository;

    public async Task<ErrorOr<DeleteProductResult>> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        if (_productRepository.GetById(command.ProductId) is not Product product)
        {
            return Error.NotFound("ProductNotFound", "Product was not found.");
        }

        _productRepository.Remove(product);

        return new DeleteProductResult();
    }
}