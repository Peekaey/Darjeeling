
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
    private readonly ILodestoneApi _lodestoneApi;
    
    public DomainService(ILogger<DomainService> logger, IMappingHelper mappingHelper, IDiscordBackendApiService discordBackendApiService, IUnitOfWork unitOfWork, ILodestoneApi lodestoneApi)
    {
        _logger = logger;
        _mappingHelper = mappingHelper;
        _discordBackendApiService = discordBackendApiService;
        _unitOfWork = unitOfWork;
        _lodestoneApi = lodestoneApi;
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
        FCGuildServer fcGuildServer = await _mappingHelper.MapRegisterFCGuildServerDTOToFCGuildServer(registerFcGuildServerDto, fcGuildMembers, fcGuildRole);

        
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

    public async Task<ServiceResult> UpdatedMemberData(ulong guildId)
    {
        var guild = await _unitOfWork.FCGuildServerRepository.GetGuildServerByDiscordGuildUid(guildId.ToString());
        if (guild == null)
        {
            return ServiceResult.AsFailure("Guild not found");
        }

        var webResult = await _lodestoneApi.GetLodestoneFreeCompanyMembers(guild.FreeCompanyId);
        
        if (webResult.Success == false || webResult.Members == null)
        {
            return ServiceResult.AsFailure("Error occured when getting Web Result from Lodestone API in GetUpdatedMemberData");
        }
        
        // Get Members
        var guildMembers = await _discordBackendApiService.GetGuildMembers(guildId);
        var matchedFcMembers = await _mappingHelper.MapLodestoneFCMembersToFCMembers(webResult.Members.ToList(), guildMembers);
        
        // TODO Investigate further on type of deletion logic based on if leave discord guild or leave FC
        // Match Members To Existing Members
        var dbGuildMembers = await _unitOfWork.FCGuildMemberRepository.GetGuildMembersByGuildId(guildId.ToString());
        var updatedFcGuildMemberList = await _mappingHelper.MapMatchedLodestoneMemberToExistingFCGuildMember(matchedFcMembers, dbGuildMembers);
        
        
        //Get Members no longer in fc & delete
        var membersNotInFc = dbGuildMembers.Where(dbm => !updatedFcGuildMemberList.UpdatedFCGuildMembers.Contains(dbm)).ToList();
        
        // Save to Database
        
        // Update Existing Members
        await _unitOfWork.FCGuildMemberRepository.UpdateRangeAsync(updatedFcGuildMemberList.UpdatedFCGuildMembers);
        // Add New Members
        await _unitOfWork.FCGuildMemberRepository.AddRangeAsync(updatedFcGuildMemberList.NewFCGuildMembers);
        // Remove Members no longer in FC

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

    public async Task<List<FCGuildMemberDTO?>> GetRegisteredFCGuildMembers(ulong guildId)
    {
        var guild = await _unitOfWork.FCGuildServerRepository.GetGuildServerByDiscordGuildUid(guildId.ToString());
        if (guild == null)
        {
            return null;
        }
        
        var registeredMembers = await _unitOfWork.FCGuildMemberRepository.GetGuildMembersByGuildId(guildId.ToString());
        
        if (registeredMembers.Count == 0)
        {
            return null;
        }
        
        var mappedDtos = await _mappingHelper.MapFCGuildMemberToFCGUildMemberDTO(registeredMembers);
        return mappedDtos;
        
    }
    
}