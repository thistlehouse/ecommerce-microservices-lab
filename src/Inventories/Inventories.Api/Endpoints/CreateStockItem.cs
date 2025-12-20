using ErrorOr;
using Inventories.Api.Errors;
using Inventories.Application.Commands.CreateStockItem;
using MediatR;

namespace Inventories.Api.Endpoints;

public sealed class CreateStockItem : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/inventory",
            async (ISender sender, StockItemRequest request) =>
        {
            CreateStockItemCommand command = new(
                request.Id,
                request.Name,
                request.Units);

            ErrorOr<Unit> response = await sender.Send(command);

            response.Match(
                item => Results.Ok(),
                errors => ApiErrors.Problem(errors));
        });
    }

    public record StockItemRequest(Guid Id, string Name, int Units);
}