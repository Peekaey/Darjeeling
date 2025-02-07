using Darjeeling.Interfaces;
using Darjeeling.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetCord;
using NetCord.Rest;

namespace Darjeeling.Helpers;

public class PermissionHelpers : IPermissionHelpers
{
    private readonly ILogger<PermissionHelpers> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly RestClient _restClient;

    
    public PermissionHelpers(ILogger<PermissionHelpers> logger, IServiceProvider serviceProvider, RestClient restClient)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _restClient = restClient;
    }
    
    public async Task<bool> IsGuildRegistered(ulong guildId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var guild = await unitOfWork.FCGuildServerRepository.GetGuildServerByDiscordGuildUid(guildId.ToString());
            if (guild == null)
            {
                return false;
            }

            return true;
        }
    }

    public async Task<List<ulong>?> GetUserRoleIds(ulong guildId ,ulong userId)
    {
        var user = await _restClient.GetGuildUserAsync(guildId, userId);
        
        if (user == null || user.RoleIds.Count == 0)
        {
            return null;
        }
        
        return user.RoleIds.ToList();
    }
    
    public async Task<bool> IsUserAdminRole(ulong guildId, List<ulong> userRoleIds)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var guildAdminRole = await unitOfWork.FCGuildRoleRepository.GetGuildRoleByDiscordGuildUid(guildId.ToString());
        
            if (guildAdminRole == null)
            {
                return false;
            }

            foreach (var roleId in userRoleIds)
            {
                if (roleId.ToString() == guildAdminRole.RoleId)
                {
                    return true;
                }
            }
        
            return false;
        }
    }
    
    public async Task<ServiceResult> CheckRegisteredGuildPermissions(ulong guildId, ulong userIds)
    {
        var isGuildRegistered = await IsGuildRegistered(guildId);
        
        if (!isGuildRegistered)
        {
            return ServiceResult.AsFailure("Guild not registered");
        }

        var userRoleIds = await GetUserRoleIds(guildId, userIds);
        
        if (userRoleIds == null)
        {
            return ServiceResult.AsFailure("User is not an admin");
        }
        
        var isUserAdminRole = await IsUserAdminRole(guildId, userRoleIds);

        if (!isUserAdminRole)
        {
            return ServiceResult.AsFailure("User is not an admin");
        }

        return ServiceResult.AsSuccess();
    }
}