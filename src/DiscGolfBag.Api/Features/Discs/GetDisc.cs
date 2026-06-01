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
                .Select(d => new
                {
                    d.Id, d.Name, d.Manufacturer, d.Plastic, d.Type,
                    d.Speed, d.Glide, d.Turn, d.Fade,
                    d.Weight, d.Color, d.ImageUrl, d.CreatedAt
                })
                .ToListAsync();

            return Results.Ok(discs);
        }).RequireAuthorization();
    }
}