using Darjeeling.Interfaces.Repositories;
using Darjeeling.Models.Entities;

namespace Darjeeling.DataContext.Repositories;

public class NameHistoryRepository : INameHistoryRepository
{
    private readonly Darjeeling.Repositories.DataContext _context;
    private readonly ILogger<NameHistoryRepository> _logger;
    
    public NameHistoryRepository(Darjeeling.Repositories.DataContext context, ILogger<NameHistoryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task AddAsync(NameHistory history)
    {
        await _context.NameHistories.AddRangeAsync(history);
    }
    
    public async Task AddRangeAsync(List<NameHistory> history)
    {
        await _context.NameHistories.AddRangeAsync(history);
    }
    
    public async Task RemoveAsync(NameHistory history)
    {
        _context.NameHistories.RemoveRange(history);
    }
    
    public async Task RemoveRangeAsync(List<NameHistory> history)
    {
        _context.NameHistories.RemoveRange(history);
    }
}