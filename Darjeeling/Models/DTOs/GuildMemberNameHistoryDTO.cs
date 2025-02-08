using Darjeeling.Models.Entities;

namespace Darjeeling.Models.DTOs;

public class GuildMemberNameHistoryDTO
{
    public List<DiscordNameHistoryDTO> DiscordNameHistories { get; set; }
    public List<LodestoneNameHistoryDTO> LodestoneNameHistories { get; set; }
}
