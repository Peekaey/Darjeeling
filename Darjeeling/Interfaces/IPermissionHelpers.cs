using NetCord;

namespace Darjeeling.Interfaces;

public interface IPermissionHelpers
{
    Task<Channel>? CanAccessChannel(ulong channelId);
}