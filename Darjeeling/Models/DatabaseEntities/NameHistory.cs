using System.ComponentModel.DataAnnotations;

namespace Darjeeling.Models.Entities;

public class NameHistory
{
    public int Id { get; set; }
    public string? DiscordUserUid { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateAdded { get; set; }
    // FK
    public int FCMemberId { get; set; }
    
    // Navigation Property
    public FCGuildMember FcGuildMember { get; set; }
}