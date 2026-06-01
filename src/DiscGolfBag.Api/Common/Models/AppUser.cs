using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Net.Http.Headers;


namespace DiscGolfBag.Api.Common.Models;

public class AppUser : IdentityUser
{
    public string DisplayName {get; set; } = string.Empty;
    public string? Bio {get; set; }
    public string? AvatarUrl {get;set;} 
    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
    
    public ICollection<Disc> Discs {get;set;} =[];

}