
using Darjeeling.Interfaces;
using Darjeeling.Models;
using Darjeeling.Models.DTOs;
using Darjeeling.Models.Entities;
using Darjeeling.Models.LodestoneEntities;

namespace Darjeeling.Services;

public class DomainService : IDomainService
{
    private readonly ILogger<DomainService> _logger;
    
    public DomainService(ILogger<DomainService> logger)
    {
        _logger = logger;
    }

    public async Task<ServiceResult> RegisterFCGuildServer(RegisterFCGuildServerDTO registerFcGuildServerDto, List<LodestoneFCMember> fcMembers)
    {
        return ServiceResult.AsSuccess();

    }
}