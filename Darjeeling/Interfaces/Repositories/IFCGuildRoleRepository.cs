using Darjeeling.Models.Entities;

namespace Darjeeling.Interfaces.Repositories;

public interface IFCGuildRoleRepository
{
    Task AddAsync(FCGuildRole role);
    Task RemoveAsync(FCGuildRole role);
}