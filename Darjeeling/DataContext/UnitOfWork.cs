using Darjeeling.Interfaces;
using Darjeeling.Interfaces.Repositories;

namespace Darjeeling.DataContext.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly Darjeeling.Repositories.DataContext _context;
    private readonly ILogger<UnitOfWork> _logger;
    
    // Repository properties
    public IFCGuildMemberRepository FCGuildMemberRepository { get; }
    public IFCGuildServerRepository FCGuildServerRepository { get; }
    public IFCGuildRoleRepository FCGuildRoleRepository { get; }
    public ILodestoneNameHistoryRepository LodestoneNameHistoryRepository { get; }
    public IDiscordNameHistoryRepository DiscordNameHistoryRepository { get; }
    
    public UnitOfWork(Darjeeling.Repositories.DataContext context, 
        ILogger<UnitOfWork> logger,
        IFCGuildMemberRepository fcGuildMemberRepository,
        IFCGuildServerRepository fcGuildServerRepository,
        IFCGuildRoleRepository fcGuildRoleRepository,
        ILodestoneNameHistoryRepository lodestoneNameHistoryRepository,
        IDiscordNameHistoryRepository discordNameHistoryRepository
        )
    {
        _context = context;
        _logger = logger;

        FCGuildMemberRepository = fcGuildMemberRepository;
        FCGuildServerRepository = fcGuildServerRepository;
        FCGuildRoleRepository = fcGuildRoleRepository;
        LodestoneNameHistoryRepository = lodestoneNameHistoryRepository;
        DiscordNameHistoryRepository = discordNameHistoryRepository;
    }
    
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
    
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
    
    public async Task BeginTransactionAsync()
    {
        await _context.Database.BeginTransactionAsync();
    }
    
    public async Task CommitTransactionAsync()
    {
        await _context.Database.CommitTransactionAsync();
    }
    
    public async Task RollbackTransactionAsync()
    {
        await _context.Database.RollbackTransactionAsync();
    }
    
    public void Dispose()
    {
        _context.Dispose();
    }
    

}