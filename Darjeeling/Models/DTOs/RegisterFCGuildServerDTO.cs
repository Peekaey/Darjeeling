using Darjeeling.Models.Entities;
using Darjeeling.Models.LodestoneEntities;

namespace Darjeeling.Models.DTOs;

public class RegisterFCGuildServerDTO
{
    public string AdminRoleId { get; set; }
    public string DiscordGuildUid { get; set; }
    public ICollection<LodestoneFCMember> FcMembers { get; set; }
    public string DiscordGuildName { get; set; }
    public string FreeCompanyName { get; set; } 
    public string AdminChannelId { get; set; }
    
}