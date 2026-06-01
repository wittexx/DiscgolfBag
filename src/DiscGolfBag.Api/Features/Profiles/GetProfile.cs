using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DiscGolfBag.Api.Common.Data;
using DiscGolfBag.Api.Common.Models;

namespace DiscGolfBag.Api.Features.Profiles;

public static class GetProfile
{
    public static void MapGetProfileEndpoint(this WebApplication app)
    {
        app.MapGet("/api/profiles/{username}/discs", async (string username, ClaimsPrincipal user, UserManager<AppUser> userManager, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Results.Unauthorized();

            var profileUser = await userManager.FindByNameAsync(username);
            if (profileUser == null) return Results.NotFound("User not found.");

            // Check if they are friends
            var areFriends = await db.Friendships.AnyAsync(f =>
                ((f.RequesterId == userId && f.AddresseeId == profileUser.Id) ||
                 (f.RequesterId == profileUser.Id && f.AddresseeId == userId))
                && f.Status == FriendshipStatus.Accepted);

            if (!areFriends) return Results.Forbid();

            var discs = await db.Discs
                .Where(d => d.OwnerId == profileUser.Id)
                .OrderByDescending(d => d.CreatedAt)
                .Select(d => new
                {
                    d.Id, d.Name, d.Manufacturer, d.Plastic, d.Type,
                    d.Speed, d.Glide, d.Turn, d.Fade,
                    d.Weight, d.Color, d.ImageUrl
                })
                .ToListAsync();

            return Results.Ok(new { profileUser.UserName, profileUser.DisplayName, Discs = discs });
        }).RequireAuthorization();
    }
}