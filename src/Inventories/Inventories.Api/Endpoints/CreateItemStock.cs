namespace Inventories.Api.Endpoints;

public sealed class CreateItemStock : IEndpoint
{
    private static List<ItemStock> _itemStocks;

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/inventory",
            (ItemStockRequest request) =>
        {
            ItemStock item = new(request.Id, request.Name, request.Units);

            _itemStocks?.Add(item);
            Console.WriteLine(item.ToString());
            return Results.Ok(item);
        });
    }

    public record ItemStockRequest(Guid Id, string Name, int Units);
    private record ItemStock(Guid Id, string Name, int Units)
    {
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Units: {Units}";
        }
    }
}