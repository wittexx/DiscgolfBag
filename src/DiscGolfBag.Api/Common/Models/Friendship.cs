namespace DiscGolfBag.Api.Common.Models;

public enum FriendshipStatus
{
    Pending,
    Accepted,
    Declined,
   
}


public class Friendship
{
    public int Id {get;set;}

    public string RequesterId {get;set;} = string.Empty;
    public AppUser Requester {get;set;} = null!;

    public string AddresseeId {get;set;} = string.Empty;
    public AppUser Addressee {get;set;} = null!;

    public FriendshipStatus Status {get;set;} = FriendshipStatus.Pending;
    public DateTime CreatedAt {get;set;} = DateTime.UtcNow;
 
}