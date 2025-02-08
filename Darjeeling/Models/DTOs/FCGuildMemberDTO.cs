namespace Darjeeling.Models.DTOs;

public class FCGuildMemberDTO
{
    public string DiscordUsername { get; set; }
    public string DiscordNickname { get; set; }
    public string DiscordGuildNickName { get; set; }
    
    public string LodestoneCharacterId { get; set; }
    public string LodestoneFirstName { get; set; }
    public string LodestoneLastName { get; set; }
    public string LodestoneFullName => LodestoneFirstName + " " + LodestoneLastName;
    
}