using Darjeeling.Models;
using NetCord;

namespace Darjeeling.Interfaces;

public interface IPermissionHelpers
{
    Task<bool> IsGuildRegistered(ulong guildId);
    Task<ServiceResult> CheckRegisteredGuildPermissions(ulong guildId, ulong userId, ulong interactionChannelId);
}