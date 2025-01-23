using Darjeeling.Models.DTOs;
using Darjeeling.Models.Entities;
using Darjeeling.Models.LodestoneEntities;

namespace Darjeeling.Interfaces;

public interface IMappingHelper
{
    FCGuildServer MapFCGuildServerDTOToFCGuildServer(RegisterFCGuildServerDTO registerFcGuildServerDto);
    Task<List<FCMember>> MapLodestoneFCMembersToFCMembers(ICollection<LodestoneFCMember> lodestoneFcMembers);
}