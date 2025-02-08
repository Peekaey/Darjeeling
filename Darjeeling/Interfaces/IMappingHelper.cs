using Darjeeling.Models;
using Darjeeling.Models.DTOs;
using Darjeeling.Models.Entities;
using Darjeeling.Models.LodestoneEntities;
using NetCord;

namespace Darjeeling.Interfaces;

public interface IMappingHelper
{
    Task<List<FCGuildMember>> MapLodestoneFCMembersToFCMembers(List<LodestoneFCMember> lodestoneFcMembers, List<GuildUser> guildUsers);

    Task<FCGuildRole> MapRegisterFCGuildServerDTOToFCGuildRoleAdmin(RegisterFCGuildServerDTO registerFcGuildServerDto,
        Role guildRole);

    Task<FCGuildServer> MapRegisterFCGuildServerDTOToFCGuildServer(
        RegisterFCGuildServerDTO registerFcGuildServerDto, List<FCGuildMember> fcGuildMembers, FCGuildRole fcGuildRole);

    Task<UpdatedFCMembersResult> MapMatchedLodestoneMemberToExistingFCGuildMember(List<FCGuildMember> lodestoneFcGuildMembers, 
        List<FCGuildMember> existingFcGuildMembers, int guildId);

    Task<List<FCGuildMemberDTO>> MapFCGuildMemberToFCGUildMemberDTO(List<FCGuildMember> fcGuildMembers);
}