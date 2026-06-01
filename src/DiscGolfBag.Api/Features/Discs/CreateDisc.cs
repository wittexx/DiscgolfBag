using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DiscGolfBag.Api.Common.Data;
using DiscGolfBag.Api.Common.Models;

namespace DiscGolfBag.Api.Features.Discs;

public static class CreateDisc
{
    public static void MapCreateDiscEndpoint(this WebApplication app)
    {
        app.MapPost("/api/discs", async (HttpContext context, AppDbContext db) =>
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Results.Unauthorized();

            
            var discCount = await db.Discs.CountAsync(d => d.OwnerId == userId);
            if (discCount >= 45)
            {
                return Results.BadRequest("You can only have 45 discs in your bag, 20 if you ask gannon buhr.");
            }

            var form = await context.Request.ReadFormAsync();

            var disc = new Disc
            {
                OwnerId = userId,
                Name = form["name"].ToString(),
                Manufacturer = form["manufacturer"].ToString(),
                Plastic = form["plastic"].ToString(),
                Type = Enum.Parse<DiscType>(form["type"].ToString()),
                Speed = double.Parse(form["speed"].ToString()),
                Glide = double.Parse(form["glide"].ToString()),
                Turn = double.Parse(form["turn"].ToString()),
                Fade = double.Parse(form["fade"].ToString()),
                Weight = int.TryParse(form["weight"].ToString(), out var w) ? w : null,
                Color = form["color"].ToString()
            };

            
            var file = form.Files.GetFile("image");
            if (file is { Length: > 0 and <= 5 * 1024 * 1024 })
            {
                var uploadsDir = Path.Combine("wwwroot", "uploads");
                Directory.CreateDirectory(uploadsDir);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(uploadsDir, fileName);

                await using var stream = File.Create(filePath);
                await file.CopyToAsync(stream);

                disc.ImageUrl = $"/uploads/{fileName}";
            }

            db.Discs.Add(disc);
            await db.SaveChangesAsync();

            return Results.Ok(disc);
        }).RequireAuthorization();
    }
}