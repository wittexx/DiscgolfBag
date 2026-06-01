using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DiscGolfBag.Api.Common.Data;
using DiscGolfBag.Api.Common.Models;

namespace DiscGolfBag.Api.Features.Friends;


public static class SendRequest
{
    public static void MapSendRequestEndpoint(this WebApplication app)
    {
        app.MapPost("/api/friends/request/{username}", async (string username, ClaimsPrincipal user, AppDbContext db, UserManager<AppUser> userManager) =>
        {
            var requesterId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (requesterId == null) return Results.Unauthorized();

            var addressee = await userManager.FindByNameAsync(username);
            if (addressee == null) return Results.NotFound("User not found.");
            if (addressee.Id == requesterId) return Results.BadRequest("You cannot send a friend request to yourself.");


            var existing = await db.Friendships.FirstOrDefaultAsync(f =>
                (f.RequesterId == requesterId && f.AddresseeId == addressee.Id) ||
                (f.RequesterId == addressee.Id && f.AddresseeId == requesterId));

            if (existing != null) return Results.BadRequest("Friend request already exists.");

            var friendship = new Friendship
            {
                RequesterId = requesterId,
                AddresseeId = addressee.Id,

            };
            db.Friendships.Add(friendship);
            await db.SaveChangesAsync();

            return Results.Ok("Friend request sent.");
        }).RequireAuthorization();
    }
}
