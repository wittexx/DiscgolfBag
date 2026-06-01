using Microsoft.AspNetCore.Identity;
using DiscGolfBag.Api.Common.Models;

namespace DiscGolfBag.Api.Features.Auth;

public static class Register
{
    public record RegisterRequest(string Username, string Email, string Password, string DisplayName);
    public record RegisterResponse(string Id, string Username, string DisplayName);

    public static void MapRegisterEndpoint(this WebApplication app)
    {
        app.MapPost("/api/auth/register", async (RegisterRequest request, UserManager<AppUser> userManager) =>
        {
            var user = new AppUser
            {
                UserName = request.Username,
                Email = request.Email,
                DisplayName = request.DisplayName
            };

            var result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.Ok(new RegisterResponse(user.Id, user.UserName!, user.DisplayName));
        });
    }
}
