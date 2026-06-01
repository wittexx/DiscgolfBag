using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using DiscGolfBag.Api.Common.Models;


namespace DiscGolfBag.Api.Features.Auth;

public static class Me
{
    public record MeResponse(string Id, string Username, string DisplayName, string? Bio, string? Email);

    public static void MapMeEndpoint(this WebApplication app)
    {
        app.MapGet("/api/auth/me", async (ClaimsPrincipal user, UserManager<AppUser> userManager) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Results.Unauthorized();
            }
            var AppUser = await userManager.FindByIdAsync(userId);
            if (AppUser == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(new MeResponse(AppUser.Id, AppUser.UserName!, AppUser.DisplayName, AppUser.Bio, AppUser.Email));
        }).RequireAuthorization();
    }
}