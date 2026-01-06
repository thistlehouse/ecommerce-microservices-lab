using Microsoft.AspNetCore.Authorization;

namespace Inventories.Api.Permissions;

public sealed class PermissionHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        bool hasClaim = context.User.Claims.Any(claim =>
            claim.Type == "permissions" &&
            claim.Value == requirement.Permission);

        if (hasClaim)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}