using Darjeeling.Models.Entities;

namespace Darjeeling.Models.DTOs;

public class RegisterFCGuildServerDTO
{
    public string AdminRoleId { get; set; }
    public string DiscordGuildUid { get; set; }
    public ICollection<FCMember> FcMembers { get; set; }
    public string DiscordGuildName { get; set; }
    public string FreeCompanyName { get; set; }
    
}