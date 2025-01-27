using Darjeeling.Models.DTOs;
using Darjeeling.Models.Entities;
using Darjeeling.Models.LodestoneEntities;
using NetCord;

namespace Darjeeling.Interfaces;

public interface IMappingHelper
{
    Task<List<FCGuildMember>> MapLodestoneFCMembersToFCMembers(List<LodestoneFCMember> lodestoneFcMembers);

    Task<FCGuildRole> MapRegisterFCGuildServerDTOToFCGuildRoleAdmin(RegisterFCGuildServerDTO registerFcGuildServerDto,
        Role guildRole);

    Task<FCGuildServer> MapRegisterFCGuildServerDTOToFCGuildServer(RegisterFCGuildServerDTO registerFcGuildServerDto, List<FCGuildMember> fcGuildMembers);
}