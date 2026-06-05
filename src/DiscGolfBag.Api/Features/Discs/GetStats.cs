using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using DiscGolfBag.Api.Common.Data;
using DiscGolfBag.Api.Common.Models;

namespace DiscGolfBag.Api.Features.Discs;

public static class GetStats
{
    public record BagStats(
        int TotalDiscs,
        int MaxDiscs,
        int Putters,
        int Midranges,
        int FairwayDrivers,
        int DistanceDrivers,
        double AverageSpeed,
        double AverageTurn,
        double AverageFade,
        string Stability,
        List<string> Suggestions
    );

    public static void MapGetStatsEndpoint(this WebApplication app)
    {
        app.MapGet("/api/discs/stats", async (ClaimsPrincipal user, AppDbContext db) =>
        {
            var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Results.Unauthorized();

            var discs = await db.Discs
                .Where(d => d.OwnerId == userId)
                .ToListAsync();

            if (discs.Count == 0)
                return Results.Ok(new BagStats(0, 45, 0, 0, 0, 0, 0, 0, 0, "N/A",
                    ["Add some discs to get your bag analysis!"]));

            
            var putters  = discs.Count(d => d.Type == DiscType.Putter);
            var midranges = discs.Count(d => d.Type == DiscType.Midrange);
            var fairways  = discs.Count(d => d.Type == DiscType.Fairway);
            var drivers   = discs.Count(d => d.Type == DiscType.DistanceDriver);

            // Calculate averages across the whole bag
            var avgSpeed = Math.Round(discs.Average(d => d.Speed), 1);
            var avgTurn  = Math.Round(discs.Average(d => d.Turn),  1);
            var avgFade  = Math.Round(discs.Average(d => d.Fade),  1);

            
            var stability = avgTurn switch
            {
                > -0.5 => "Overstable dominant",
                < -1.5 => "Understable dominant",
                _      => "Balanced"
            };

            
            var suggestions = new List<string>();

            if (putters == 0)
                suggestions.Add("No putters — add one for approach shots and short putts.");
            if (midranges == 0)
                suggestions.Add("No midranges — great for accurate shots on shorter holes.");
            if (fairways == 0)
                suggestions.Add("No fairway drivers — useful for controlled distance off the tee.");
            if (drivers == 0)
                suggestions.Add("No distance drivers — consider one for maximum distance shots.");

            if (discs.All(d => d.Turn >= 0))
                suggestions.Add("All discs are overstable — add an understable disc for hyzer-flip shots.");
            else if (discs.All(d => d.Turn <= -2))
                suggestions.Add("All discs are understable — add overstable discs for windy conditions.");

            if (discs.All(d => d.Speed >= 9))
                suggestions.Add("Only high-speed discs in your bag — slower discs offer more control.");

            if (discs.Count >= 40)
                suggestions.Add($"Bag is nearly full ({discs.Count}/45) — choose your next disc wisely!");

            if (suggestions.Count == 0)
                suggestions.Add("Your bag looks well-balanced — nice work!");

            return Results.Ok(new BagStats(
                discs.Count, 45,
                putters, midranges, fairways, drivers,
                avgSpeed, avgTurn, avgFade,
                stability, suggestions
            ));
        }).RequireAuthorization();
    }
}
