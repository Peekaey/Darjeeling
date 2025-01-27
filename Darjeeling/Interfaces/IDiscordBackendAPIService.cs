using NetCord;

namespace Darjeeling.Interfaces;

public interface IDiscordBackendApiService
{
    Task<Role>? GetRoleDetails(ulong guildId, ulong roleId);
    Task<Channel>? GetChannel(ulong channelId);
}