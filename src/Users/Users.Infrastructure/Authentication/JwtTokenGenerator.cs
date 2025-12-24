using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Users.Application.Common.Abstractions.Services;
using Users.Domain;
using Users.Domain.Enums;
using Users.Domain.Permissions;
using Users.Infrastructure.Authorization;

namespace Users.Infrastructure.Authentication;

public class JwtTokenGenerator(
    IDateTimeProvider dateTimeProvider,
    IOptions<JwtSettings> jwtSetting,
    IOptions<PermissionSettings> permissionSettings)
    : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly JwtSettings _jwtSettings = jwtSetting.Value;
    private readonly PermissionSettings _permissionSettings = permissionSettings.Value;

    public string GenerateToken(User user)
    {
        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
            SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtPermissionClaimNames.UserType, user.UserType.ToString()),
            new Claim(JwtPermissionClaimNames.Permissions, GetStringPermissions(user.UserType, _permissionSettings)),
        ];

        JwtSecurityToken securityToken = new(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            signingCredentials: signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    private string GetStringPermissions(UserType userType, PermissionSettings permissionSettings)
    {
        string[] permissions = GetPermissions(userType, permissionSettings);
        StringBuilder sb = new();

        foreach (string permission in permissions)
        {
            sb.Append(permission);
            sb.Append(' ');
        }

        return sb.ToString().TrimEnd();
    }

    private string[] GetPermissions(UserType userType, PermissionSettings permissionSettings)
    {
        return userType switch
        {
            UserType.Customer => permissionSettings.CustomerPermissions,
            UserType.Admin => permissionSettings.AdminPermissions,
            _ => throw new ArgumentException($"Unknown type of user {nameof(userType)}"),
        };
    }
}