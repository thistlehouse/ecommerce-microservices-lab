using ErrorOr;
using MediatR;
using Products.Application.Common.Abstractions;
using Products.Application.Common.Abstractions.Services.StockItems;
using Products.Contracts.Products;
using Products.Domain.Products;

namespace Products.Application.Commands.CreateProducts;

public sealed class CreateProductCommandHandler(
    IProductRepository productRepository,
    IStockItemService stockItemService)
    : IRequestHandler<CreateProductCommand, ErrorOr<CreateProductResult>>
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IStockItemService _stockItemService = stockItemService;

    public async Task<ErrorOr<CreateProductResult>> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;

        Product product = Product.Create(
            command.Name,
            command.Description,
            command.Price);

        // Todo
        // this will change, but for now will suffice.
        try
        {
            await _stockItemService.CreateStockItem(product);
        }
        catch (HttpRequestException e)
        {
            // Todo
            // use proper log afterwards
            Console.WriteLine("An HttpRequestException has occurred");
            Console.WriteLine($"Message: {e.Message}");
            Console.WriteLine($"Message: {e.TargetSite}");
        }
        finally
        {
            _productRepository.Add(product);
        }

        return new CreateProductResult(product.Id);
    }
}
