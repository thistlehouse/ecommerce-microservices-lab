using ErrorOr;
using MediatR;

namespace Users.Application.Authentication.Command.ResendEmailConfirmation;

public sealed record ResendEmailConfirmationCommand(string UserEmail) : IRequest<ErrorOr<Unit>>;