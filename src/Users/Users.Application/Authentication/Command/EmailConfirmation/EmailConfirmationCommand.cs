using ErrorOr;
using MediatR;

namespace Users.Application.Authentication.Command.EmailConfirmation;

public sealed record EmailConfirmationCommand(string Code) : IRequest<ErrorOr<Unit>>;