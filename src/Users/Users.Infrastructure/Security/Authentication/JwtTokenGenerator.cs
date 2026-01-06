using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Users.Application.Common.Abstractions.Authorization;
using Users.Application.Common.Abstractions.Services;
using Users.Domain;
using Users.Domain.Enums;
using Users.Domain.Permissions;

namespace Users.Infrastructure.Authentication;

public class JwtTokenGenerator(
    IDateTimeProvider dateTimeProvider,
    IOptions<JwtSettings> jwtSetting,
    IPermissionProvider permissionProvider)
    : IJwtTokenGenerator
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly JwtSettings _jwtSettings = jwtSetting.Value;
    private readonly IPermissionProvider _permissionProvider = permissionProvider;

    public string GenerateToken(Entity entity)
    {
        SigningCredentials signingCredentials = GetSigningCredentials(_jwtSettings.Secret);
        List<Claim> claims = new();

        if (entity is User user)
        {
            AddClaimsToTypeUse(claims, user);
        }

        if (entity is ServiceIdentity service)
        {
            AddClaimsToTypeService(claims, service);
        }

        AddPermissions(entity.ClientType, claims);

        JwtSecurityToken securityToken = GetJwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims.ToArray(),
            _dateTimeProvider.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            signingCredentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    private List<Claim> AddClaimsToTypeUse(List<Claim> claims, User user)
    {
        claims.AddRange(
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.GivenName, user.FirstName),
            new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ]);

        return claims;
    }

    private List<Claim> AddClaimsToTypeService(List<Claim> claims, ServiceIdentity service)
    {
        claims.AddRange(
        [
            new Claim(JwtRegisteredClaimNames.Sub, service.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, service.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        ]);

        return claims;
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

    private void AddPermissions(ClientType entityType, List<Claim> claims)
    {
        IReadOnlyCollection<string> permissions = _permissionProvider.GetPermissions(entityType);
        foreach (string permission in permissions)
        {
            claims.Add(new Claim(JwtPermissionClaimNames.Permissions, permission));
        }
    }
}