namespace Darjeeling.Models.Entities;

public class FCGuildRole
{
    public int Id { get; set; }
    public string RoleId { get; set; }
    public string RoleName { get; set; }
    public string DiscordGuildUid { get; set; }
    // Remove FreeCompanyId in Next Migration
    public DateTime DateCreated { get; set; }
    public RoleType RoleType { get; set; }
    
    // FK
    public int FCGuildServerId { get; set; }
    // Navigation Property
    public FCGuildServer FCGuildServer { get; set; }
}