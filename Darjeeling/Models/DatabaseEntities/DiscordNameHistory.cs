namespace Darjeeling.Models.Entities;

public class DiscordNameHistory
{
    public int Id { get; set; }
    public string DiscordUsername { get; set; }
    public string DiscordNickName { get; set; }
    public DateTime DateAdded { get; set; }
    // FK
    public int FCMemberId { get; }
    
    // Navigation Property
    public FCGuildMember FcGuildMember { get; set; }
    
}