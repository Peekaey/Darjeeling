using Darjeeling.Models.Entities;

namespace Darjeeling.Interfaces.Repositories;

public interface INameHistoryRepository
{
    Task AddAsync(NameHistory history);
    Task AddRangeAsync(List<NameHistory> history);
    Task RemoveAsync(NameHistory history);
    Task RemoveRangeAsync(List<NameHistory> history);
}