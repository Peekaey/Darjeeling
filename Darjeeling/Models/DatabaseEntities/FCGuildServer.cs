namespace Darjeeling.Models.Entities;

public class FCGuildServer
{
    public int Id { get; set; }
    public string AdminRoleId { get; set; }
    public string DiscordGuildUid { get; set; }
    public DateTime DateCreated { get; set; }
    public ICollection<FCMember> FCMembers { get; set; }
    
    public string DiscordGuildName { get; set; }
    public string FreeCompanyName { get; set; }
    
    // Role to control who can run administrative commands
    public FCGuildRole FCAdminRole { get; set; }
    
}