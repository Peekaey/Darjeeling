using Darjeeling.Models.Entities;

namespace Darjeeling.Interfaces.Repositories;

public interface IFCGuildServerRepository
{
    Task AddAsync(FCGuildServer server);
    Task RemoveAsync(FCGuildServer server);
}