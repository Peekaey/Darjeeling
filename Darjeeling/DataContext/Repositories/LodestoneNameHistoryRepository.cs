using Darjeeling.Interfaces.Repositories;
using Darjeeling.Models.Entities;

namespace Darjeeling.DataContext.Repositories;

public class LodestoneNameHistoryRepository : ILodestoneNameHistoryRepository
{
    private readonly Darjeeling.Repositories.DataContext _context;
    private readonly ILogger<LodestoneNameHistoryRepository> _logger;
    
    public LodestoneNameHistoryRepository(Darjeeling.Repositories.DataContext context, ILogger<LodestoneNameHistoryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task AddRangeAsync(List<LodestoneNameHistory> history)
    {
        await _context.LodestoneNameHistories.AddRangeAsync(history);
    }
    
    
    public async Task RemoveRangeAsync(List<LodestoneNameHistory> history)
    {
        _context.LodestoneNameHistories.RemoveRange(history);
    }
}