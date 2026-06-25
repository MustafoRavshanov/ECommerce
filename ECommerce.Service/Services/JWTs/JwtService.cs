using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ECommerce.Service.Services.JWTs;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public string GenerateToken(User user)
    {
        var permissions = user.Role.RolePermissions
            .Select(rp => Enum.GetName(typeof(Permission), rp.Permission)!)
            .ToList();

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
        new Claim(ClaimTypes.Role, user.Role.Name),

        new Claim("permissions", JsonSerializer.Serialize(permissions))
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]!));

        var token = new JwtSecurityToken(
            issuer: configuration["JwtSettings:Issuer"],
            audience: configuration["JwtSettings:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(
                double.Parse(configuration["JwtSettings:ExpiresInHours"]!)),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
