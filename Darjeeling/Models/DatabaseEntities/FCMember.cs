namespace Darjeeling.Models.Entities;

public class FCMember
{
    public int Id { get; set; }
    public ulong DiscordId { get; set; }
    public string? DiscordUsername { get; set; }
    public string? LodestoneId { get; set; }
    public DateTime DateCreated { get; set; }
    
    
}