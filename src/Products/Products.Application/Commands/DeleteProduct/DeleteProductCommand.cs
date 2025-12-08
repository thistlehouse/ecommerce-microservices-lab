using ErrorOr;
using MediatR;

namespace Products.Application.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid ProductId) : IRequest<ErrorOr<DeleteProductResult>>;