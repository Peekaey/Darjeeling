using System.ComponentModel.DataAnnotations;

namespace Darjeeling.Models.Entities;

public class LodestoneNameHistory
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateAdded { get; set; }
    // FK
    public int FCMemberId { get; set; }
    
    // Navigation Property
    public FCGuildMember FcGuildMember { get; set; }
}