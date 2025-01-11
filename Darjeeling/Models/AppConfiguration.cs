using NetCord;

namespace Darjeeling.Models;

public class AppConfiguration
{
    public string botToken { get; set; } = string.Empty;
    public IEntityToken EntityToken => new BotToken(botToken);
    public ulong ChannelId { get; set; }
}