using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DiscGolfBag.Api.Common.Data;

namespace DiscGolfBag.Api.Features.Discs;

public static class GetDiscs
{
    public static void MapGetDiscsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/discs", async (ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Results.Unauthorized();

            var discs = await db.Discs
                .Where(d => d.OwnerId == userId)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            return Results.Ok(discs);
        }).RequireAuthorization();
    }
}