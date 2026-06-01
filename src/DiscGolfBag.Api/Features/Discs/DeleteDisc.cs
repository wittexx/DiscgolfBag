using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DiscGolfBag.Api.Common.Data;

namespace DiscGolfBag.Api.Features.Discs;

public static class DeleteDisc
{
    public static void MapDeleteDiscEndpoint(this WebApplication app)
    {
        app.MapDelete("/api/discs/{id:int}", async (int id, ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Results.Unauthorized();

            var disc = await db.Discs.FirstOrDefaultAsync(d => d.Id == id && d.OwnerId == userId);
            if (disc == null) return Results.NotFound();

            // Delete image file if exists
            if (!string.IsNullOrEmpty(disc.ImageUrl))
            {
                var filePath = Path.Combine("wwwroot", disc.ImageUrl.TrimStart('/'));
                if (File.Exists(filePath)) File.Delete(filePath);
            }

            db.Discs.Remove(disc);
            await db.SaveChangesAsync();

            return Results.Ok();
        }).RequireAuthorization();
    }
}