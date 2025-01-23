using Darjeeling.Models;
using Darjeeling.Models.DTOs;
using Darjeeling.Models.LodestoneEntities;

namespace Darjeeling.Interfaces;

public interface IDomainService
{
    Task<ServiceResult> RegisterFCGuildServer(RegisterFCGuildServerDTO registerFcGuildServerDto, List<LodestoneFCMember> fcMembers);
}