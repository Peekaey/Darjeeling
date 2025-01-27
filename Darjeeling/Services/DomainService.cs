
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


    public async Task<ServiceResult> RegisterFCGuildServer(RegisterFCGuildServerDTO registerFcGuildServerDto)
    {
        if (!ulong.TryParse(registerFcGuildServerDto.AdminRoleId, out var ulongGuildRoleId) ||
            !ulong.TryParse(registerFcGuildServerDto.DiscordGuildUid, out var ulongGuildId))
        {
            return ServiceResult.AsFailure("Invalid Role ID or Guild ID");
        }
        
        var guildRole =  await _discordBackendApiService.GetRoleDetails(ulongGuildId,ulongGuildRoleId);
        
        if (guildRole == null)
        {
            return ServiceResult.AsFailure("Role not found or lack of permission to access role details");
        }
        
        List<FCGuildMember> fcGuildMembers = await _mappingHelper.MapLodestoneFCMembersToFCMembers(registerFcGuildServerDto.FcMembers.ToList());
        FCGuildRole fcGuildRole = await _mappingHelper.MapRegisterFCGuildServerDTOToFCGuildRoleAdmin(registerFcGuildServerDto, guildRole);
        FCGuildServer fcGuildServer = await _mappingHelper.MapRegisterFCGuildServerDTOToFCGuildServer(registerFcGuildServerDto, fcGuildMembers);

        // Save to database
        
        await _unitOfWork.FCGuildServerRepository.AddAsync(fcGuildServer);
        await _unitOfWork.FCGuildMemberRepository.AddRangeAsync(fcGuildMembers);
        
        // await _unitOfWork.FCGuildRoleRepository.AddAsync(fcGuildRole);
        // await _unitOfWork.NameHistoryRepository.AddRangeAsync(fcGuildMembers.SelectMany(x => x.NameHistories).ToList());

        try
        {
            await _unitOfWork.SaveChangesAsync();
        } catch (Exception e)
        {
            _logger.LogError(e, "Error saving to database");
            return ServiceResult.AsFailure("Error saving to database");
        }
        
        
        return ServiceResult.AsSuccess();

    }
}