using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Darjeeling.Models.Entities;

public class FCGuildMember
{
    public int Id { get; set; }
    // Nullable during testing/dev
    public string? DiscordUserUId { get; set; }
    public string? LodestoneId { get; set; }
    public DateTime DateCreated { get; set; }
    public ICollection<LodestoneNameHistory> LodestoneNameHistories { get; set; }
    public ICollection<DiscordNameHistory> DiscordNameHistories { get; set; }
    // FK
    public int FCGuildServerId { get; set; }
    // Navigation Property
    public FCGuildServer FCGuildServer { get; set; }
}