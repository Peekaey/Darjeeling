
using Darjeeling.Helpers;
using Darjeeling.Interfaces;
using Darjeeling.Models;
using Darjeeling.Models.DTOs;
using Darjeeling.Models.Entities;
using Darjeeling.Models.LodestoneEntities;

namespace Darjeeling.Services;

public class DomainService : IDomainService
{
    private readonly ILogger<DomainService> _logger;
    private readonly IMappingHelper _mappingHelper;
    private readonly IDiscordBackendApiService _discordBackendApiService;
    private readonly IUnitOfWork _unitOfWork;
    
    public DomainService(ILogger<DomainService> logger, IMappingHelper mappingHelper, IDiscordBackendApiService discordBackendApiService, Interfaces.IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _mappingHelper = mappingHelper;
        _discordBackendApiService = discordBackendApiService;
        _unitOfWork = unitOfWork;
    }


    public async Task<RegisterServiceResult> RegisterFCGuildServer(RegisterFCGuildServerDTO registerFcGuildServerDto)
    {
        if (!ulong.TryParse(registerFcGuildServerDto.AdminRoleId, out var ulongGuildRoleId) ||
            !ulong.TryParse(registerFcGuildServerDto.DiscordGuildUid, out var ulongGuildId))
        {
            return RegisterServiceResult.AsFailure("Invalid Role ID or Guild ID");
        }
        
        var guildRole =  await _discordBackendApiService.GetRoleDetails(ulongGuildId,ulongGuildRoleId);
        
        if (guildRole == null)
        {
            return RegisterServiceResult.AsFailure("Role not found or lack of permission to access role details");
        }
        
        var guildMembers = await _discordBackendApiService.GetGuildMembers(ulongGuildId);
        
        List<FCGuildMember> fcGuildMembers = await _mappingHelper.MapLodestoneFCMembersToFCMembers(registerFcGuildServerDto.FcMembers.ToList(), guildMembers);
        FCGuildRole fcGuildRole = await _mappingHelper.MapRegisterFCGuildServerDTOToFCGuildRoleAdmin(registerFcGuildServerDto, guildRole);
        FCGuildServer fcGuildServer = await _mappingHelper.MapRegisterFCGuildServerDTOToFCGuildServer(registerFcGuildServerDto, fcGuildMembers);

        
        // Save to database
        await _unitOfWork.FCGuildServerRepository.AddAsync(fcGuildServer);
        await _unitOfWork.FCGuildMemberRepository.AddRangeAsync(fcGuildMembers);
        
        try
        {
            await _unitOfWork.SaveChangesAsync();
        } catch (Exception e)
        {
            _logger.LogError(e, "Error saving to database");
            return RegisterServiceResult.AsFailure("Error saving to database");
        }
        return RegisterServiceResult.AsSuccess(fcGuildMembers.Count());
    }
}