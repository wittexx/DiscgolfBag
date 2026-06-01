using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DiscGolfBag.Api.Common.Data;
using DiscGolfBag.Api.Common.Models;

namespace DiscGolfBag.Api.Features.Friends;

public static class DeclineRequest
{
    public static void MapDeclineRequestEndpoint(this WebApplication app)
    {
        app.MapPut("/api/friends/request/{id:int}/decline", async (int id, ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Results.Unauthorized();

            var friendship = await db.Friendships.FirstOrDefaultAsync(f => f.Id == id && f.AddresseeId == userId && f.Status == FriendshipStatus.Pending);
            if (friendship == null) return Results.NotFound();

            friendship.Status = FriendshipStatus.Declined;
            await db.SaveChangesAsync();

            return Results.Ok("Friend request declined.");
        }).RequireAuthorization();
    }
}