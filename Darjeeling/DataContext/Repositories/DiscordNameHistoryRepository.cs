using Darjeeling.Interfaces.Repositories;
using Darjeeling.Models.Entities;
using Microsoft.Extensions.Logging;

namespace Darjeeling.DataContext.Repositories;

public class DiscordNameHistoryRepository : IDiscordNameHistoryRepository
{
    private readonly Darjeeling.Repositories.DataContext _context;
    private readonly ILogger<DiscordNameHistoryRepository> _logger;
    
    public DiscordNameHistoryRepository(Darjeeling.Repositories.DataContext context, ILogger<DiscordNameHistoryRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task AddRangeAsync(List<DiscordNameHistory> history)
    {
        await _context.DiscordNameHistories.AddRangeAsync(history);
    }
    
    public async Task RemoveRangeAsync(List<DiscordNameHistory> history)
    {
        _context.DiscordNameHistories.RemoveRange(history);
    }
}