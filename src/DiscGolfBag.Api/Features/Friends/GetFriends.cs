using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DiscGolfBag.Api.Common.Data;
using DiscGolfBag.Api.Common.Models;

namespace DiscGolfBag.Api.Features.Friends;

public static class GetFriends
{
    public static void MapGetFriendsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/friends", async (ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Results.Unauthorized();

            var friends = await db.Friendships
                .Where(f => (f.RequesterId == userId || f.AddresseeId == userId) && f.Status == FriendshipStatus.Accepted)
                .Include(f => f.Requester)
                .Include(f => f.Addressee)
                .Select(f => f.RequesterId == userId
                    ? new { f.Id, Username = f.Addressee.UserName, f.Addressee.DisplayName }
                    : new { f.Id, Username = f.Requester.UserName, f.Requester.DisplayName })
                .ToListAsync();

            return Results.Ok(friends);
        }).RequireAuthorization();
    }
}