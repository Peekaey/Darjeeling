namespace Darjeeling.Models.Entities;

public class FCGuildServer
{
    public int Id { get; set; }
    public ulong AdminRoleId { get; set; }
    public string DiscordGuildUid { get; set; }
    public DateTime DateCreated { get; set; }
    
}