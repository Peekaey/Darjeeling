using Darjeeling.Interfaces.Repositories;
using Darjeeling.Models.Entities;

namespace Darjeeling.DataContext.Repositories;

public class FCGuildMemberRepository : IFCGuildMemberRepository
{
    private readonly Darjeeling.Repositories.DataContext _context;
    private readonly ILogger<FCGuildMemberRepository> _logger;
    
    public FCGuildMemberRepository(Darjeeling.Repositories.DataContext context, ILogger<FCGuildMemberRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task AddRangeAsync(List<FCGuildMember> member)
    {
        await _context.FCMembers.AddRangeAsync(member);
    }
    
    public async Task RemoveRangeAsync(List<FCGuildMember> member)
    {
        _context.FCMembers.RemoveRange(member);
    }

}