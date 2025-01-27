using Darjeeling.Models.Entities;

namespace Darjeeling.Interfaces.Repositories;

public interface IDiscordNameHistoryRepository
{
    Task AddRangeAsync(List<DiscordNameHistory> history);
    Task RemoveRangeAsync(List<DiscordNameHistory> history);
}