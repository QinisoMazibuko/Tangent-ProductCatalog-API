using Tangent.Catalog.API.Domain.Interfaces;
using Tangent.Catalog.API.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Tangent.Catalog.API.Domain.Config;
using System.Text;

namespace Tangent.Catalog.API.Domain.Contexts;

public class SubscriptionContext : ISubscriptionContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public SubscriptionContext(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public DefaultUser GetCurrentSubscriber()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        // Retrieve the JWT from the Authorization header
        var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (string.IsNullOrEmpty(token)) return null;

        // Decode the JWT token
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

        var subscriberId = jwtToken?.Claims.FirstOrDefault(c => c.Type == "x-subscriber-id")?.Value;

        if (string.IsNullOrEmpty(subscriberId))
        {
            return null;
        }

        return new DefaultUser
        {
            SubscriberId = subscriberId,
            Name = jwtToken?.Claims.FirstOrDefault(c => c.Type == "name")?.Value ?? "Unknown",
            Email = jwtToken?.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? "Unknown"
        };
    }

    public string GenerateJwtToken(DefaultUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("x-subscriber-id", user.SubscriberId),
                new Claim("name", user.Name),
                new Claim("email", user.Email)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = jwtSettings.Issuer,
            Audience = jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
