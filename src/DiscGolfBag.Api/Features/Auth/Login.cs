using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using DiscGolfBag.Api.Common.Models;

namespace DiscGolfBag.Api.Features.Auth;

public static class Login
{
    public record LoginRequest(string Username, string Password);
    public record LoginResponse(string Token, string Id, string Username, string DisplayName);

    public static void MapLoginEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/login", async (LoginRequest request, UserManager<AppUser> userManager, IConfiguration config) =>
        {
            var user = await userManager.FindByNameAsync(request.Username);
            if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
            {
                return Results.Unauthorized();
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                config["Jwt:Key"] ?? "default_secret_key_should_be_long_and_secure"));

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Results.Ok(new LoginResponse(tokenString, user.Id, user.UserName!, user.DisplayName));
        });
    }
}
