using HomeQuest.Models;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public bool AgentIsApproved { get; set; }
    public DateTime AgentExpirationDate { get; set; }
    public DateTime  CreateDate { get; set; }
    public string? LicenseNumber { get; set; }
   public ICollection<Favorite> Favorites { get; set; }
   public ICollection<Offer> Offers { get; set; }
}
