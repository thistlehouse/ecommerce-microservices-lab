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

    public string GenerateUserToken(User user)
    {
        SigningCredentials signingCredentials = GetSigningCredentials(_jwtSettings.Secret);

        List<Claim> claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtPermissionClaimNames.UserType, user.UserType.ToString()),
        ];

        AddPermissions(user.UserType, _permissionSettings, claims);

        JwtSecurityToken securityToken = GetJwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims.ToArray(),
            _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    public string GenerateServiceToken(ServiceIdentity serviceIdentity)
    {
        SigningCredentials signingCredentials = GetSigningCredentials(serviceIdentity.ClientSecret);

        Claim[] claims =
        [
            new Claim("client_id", serviceIdentity.ClientId),
            new Claim("token_type", "service"),
            new Claim("scopes", serviceIdentity.Scopes),
        ];

        JwtSecurityToken jwtSecurityToken = GetJwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private static SigningCredentials GetSigningCredentials(string secret)
    {
        SigningCredentials signingCredentials = new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            SecurityAlgorithms.HmacSha256);

        return signingCredentials;
    }

    private static JwtSecurityToken GetJwtSecurityToken(
        string issuer,
        string audience,
        Claim[] claims,
        DateTime expire,
        SigningCredentials signingCredentials)
    {
        JwtSecurityToken securityToken = new(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expire,
            signingCredentials: signingCredentials);

        return securityToken;
    }

    private void AddPermissions(UserType userType, PermissionSettings permissionSettings, List<Claim> claims)
    {
        string[] permissions = GetPermissions(userType, permissionSettings);
        foreach (string permission in permissions)
        {
            claims.Add(new Claim(JwtPermissionClaimNames.Permissions, permission));
        }
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