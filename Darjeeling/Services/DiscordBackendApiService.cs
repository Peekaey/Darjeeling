using Darjeeling.Interfaces;
using NetCord;
using NetCord.Rest;

namespace Darjeeling.Services;

public class DiscordBackendApiService : IDiscordBackendApiService
{
    private readonly ILogger<DiscordBackendApiService> _logger;
    private readonly RestClient _restClient;
    private readonly IPermissionHelpers _permissionHelpers;
    
    public DiscordBackendApiService(ILogger<DiscordBackendApiService> logger, RestClient restClient, IPermissionHelpers permissionHelpers)
    {
        _logger = logger;
        _restClient = restClient;
        _permissionHelpers = permissionHelpers;
    }
    
    public async Task<Role>? GetRoleDetails(ulong guildId, ulong roleId)
    {
        try
        {
            var role = await _restClient.GetGuildRoleAsync(guildId,roleId);
            return role;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error while getting role details.");
            return null;
        }
        
    }
    
    public async Task<Channel>? GetChannel(ulong channelId)
    {
        try
        {
            var channel = await _restClient.GetChannelAsync(channelId);
            return channel;
        }
        catch (RestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Forbidden)
        {
            _logger.LogError($"404 Forbidden: Bot lacks access to channel: {channelId}.");
            return null;
        }
        catch (RestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            _logger.LogError($"403 Not Found: Channel ID {channelId}: does not exist.");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Unexpected error while checking access to channel: {channelId}.");
            return null;
        }
    }
    
}