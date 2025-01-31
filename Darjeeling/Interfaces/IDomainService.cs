using Darjeeling.Models;
using Darjeeling.Models.DTOs;
using Darjeeling.Models.LodestoneEntities;

namespace Darjeeling.Interfaces;

public interface IDomainService
{
    Task<RegisterServiceResult> RegisterFCGuildServer(RegisterFCGuildServerDTO registerFcGuildServerDto);
    Task<ServiceResult> UpdatedMemberData(ulong guildId);
    Task<List<FCGuildMemberDTO?>> GetRegisteredFCGuildMembers(ulong guildId);
}