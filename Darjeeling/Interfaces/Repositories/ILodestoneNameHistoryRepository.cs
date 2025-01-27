using Darjeeling.Models.Entities;

namespace Darjeeling.Interfaces.Repositories;

public interface ILodestoneNameHistoryRepository
{
    Task AddRangeAsync(List<LodestoneNameHistory> history);
    Task RemoveRangeAsync(List<LodestoneNameHistory> history);
}