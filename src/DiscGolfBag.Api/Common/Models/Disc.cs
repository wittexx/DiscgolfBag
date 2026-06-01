using System.ComponentModel.DataAnnotations;


namespace DiscGolfBag.Api.Common.Models;


public enum DiscType
{
    Putter,
    Midrange,
    Fairway,
    DistanceDriver,
}

public class Disc
{
    public int Id {get;set;}
    public string OwnerId {get;set;} = string.Empty;
    public AppUser Owner {get;set;} = null!;


    public string Name {get;set;} = string.Empty;
    public string? Manufacturer {get;set;} = string.Empty;
    public string? Plastic {get;set;}
    public DiscType Type {get;set;}

    // flight numbers
    [Range(1, 15)]
    public double Speed {get;set;}
    [Range(1, 5)]
    public double Glide {get;set;}
    [Range(-5, 1)]
    public double Turn {get;set;}
    [Range(0, 5)]
    public double Fade {get;set;}


    public int? Weight {get;set;}
    public string? Color {get;set;}
    public string? ImageUrl {get;set;}


    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get;set;} = DateTime.UtcNow;
}