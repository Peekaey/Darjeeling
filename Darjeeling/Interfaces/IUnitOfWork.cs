using Darjeeling.Interfaces.Repositories;

namespace Darjeeling.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IFCGuildMemberRepository FCGuildMemberRepository { get; }
    IFCGuildServerRepository FCGuildServerRepository { get; }
    IFCGuildRoleRepository FCGuildRoleRepository { get; }
    ILodestoneNameHistoryRepository LodestoneNameHistoryRepository { get; }

    Task<int> SaveChangesAsync();
    void SaveChanges();
    
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    
}