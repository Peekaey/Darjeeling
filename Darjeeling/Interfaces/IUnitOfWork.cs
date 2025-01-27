using Darjeeling.Interfaces.Repositories;

namespace Darjeeling.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IFCGuildMemberRepository FCGuildMemberRepository { get; }
    IFCGuildServerRepository FCGuildServerRepository { get; }
    IFCGuildRoleRepository FCGuildRoleRepository { get; }
    INameHistoryRepository NameHistoryRepository { get; }

    Task<int> SaveChangesAsync();
    void SaveChanges();
    
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    
}