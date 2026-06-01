using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DiscGolfBag.Api.Common.Data;
using DiscGolfBag.Api.Common.Models;

namespace DiscGolfBag.Api.Features.Friends;

public static class AcceptRequest
{
    public static void MapAcceptRequestEndpoint(this WebApplication app)
    {
        app.MapPut("/api/friends/request/{id:int}/accept", async (int id, ClaimsPrincipal user, AppDbContext db) =>
        {
            var UserID = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (UserID == null) return Results.Unauthorized();

            var friendship = await db.Friendships.FirstOrDefaultAsync(f => f.Id == id && f.AddresseeId == UserID);
            if (friendship == null) return Results.NotFound("Friend request not found.");

            friendship.Status = FriendshipStatus.Accepted;
            await db.SaveChangesAsync();

            return Results.Ok("Friend request accepted.");

        }).RequireAuthorization();
    }
}