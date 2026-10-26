using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using WorkForce360.API.Models;
using WorkForce360.API.Services;

namespace WorkForce360.API.Tests;

public class JwtTokenServiceTests
{
    [Fact]
    public void CreateToken_ContainsUserIdentityRoleAndJwtSettings()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "test-signing-key-that-is-at-least-thirty-two-characters",
                ["Jwt:Issuer"] = "WorkForce360.Tests",
                ["Jwt:Audience"] = "WorkForce360.TestClient",
                ["Jwt:AccessTokenMinutes"] = "30"
            })
            .Build();
        var service = new JwtTokenService(configuration);
        var user = new AppUser { Id = 12, Email = "hr@example.test", Role = UserRoles.HR };
        var beforeCreate = DateTime.UtcNow;

        var response = service.CreateToken(user);
        var token = new JwtSecurityTokenHandler().ReadJwtToken(response.AccessToken);

        Assert.Equal("Bearer", response.TokenType);
        Assert.Equal(user.Email, response.Email);
        Assert.Equal(UserRoles.HR, response.Role);
        Assert.Equal("WorkForce360.Tests", token.Issuer);
        Assert.Contains("WorkForce360.TestClient", token.Audiences);
        Assert.Equal("12", token.Claims.Single(claim => claim.Type == ClaimTypes.NameIdentifier).Value);
        Assert.Equal(user.Email, token.Claims.Single(claim => claim.Type == ClaimTypes.Email).Value);
        Assert.Equal(UserRoles.HR, token.Claims.Single(claim => claim.Type == ClaimTypes.Role).Value);
        Assert.InRange(response.ExpiresAtUtc, beforeCreate.AddMinutes(29), beforeCreate.AddMinutes(31));
    }
}
