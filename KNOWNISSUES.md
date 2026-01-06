# Service to Service Auth
Product -> Inventory not working, returns 401.
Removed until the bug is found.

Seems like the lines bellow were causing the problem

```csharp
new Claim(JwtPermissionClaimNames.UserType, user.ClientType.ToString())

new Claim(JwtPermissionClaimNames.ServiceType, service.ClientType.ToString())
```