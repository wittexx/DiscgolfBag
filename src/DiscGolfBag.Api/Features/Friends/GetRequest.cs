using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DiscGolfBag.Api.Common.Data;
using DiscGolfBag.Api.Common.Models;

namespace DiscGolfBag.Api.Features.Friends;

public static class GetRequests
{
    public static void MapGetRequestsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/friends/requests", async (ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Results.Unauthorized();

            var requests = await db.Friendships
                .Where(f => f.AddresseeId == userId && f.Status == FriendshipStatus.Pending)
                .Include(f => f.Requester)
                .Select(f => new { f.Id, Username = f.Requester.UserName, f.Requester.DisplayName })
                .ToListAsync();

            return Results.Ok(requests);
        }).RequireAuthorization();
    }
}