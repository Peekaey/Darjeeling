using Darjeeling.Interfaces.Repositories;
using Darjeeling.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
    
    public async Task UpdateRangeAsync(List<FCGuildMember> member)
    {
        _context.FCMembers.UpdateRange(member);
    }

    public async Task<List<FCGuildMember>> GetGuildMembersByGuildId(string guildId)
    {
        return await _context.FCMembers
            .Include(fcg => fcg.FCGuildServer)
            .Include(fcg => fcg.DiscordNameHistories)
            .Include(fcg => fcg.LodestoneNameHistories)
            .Where(fcg => fcg.FCGuildServer.DiscordGuildUid == guildId).ToListAsync<FCGuildMember>();
    }
    
    public async Task<FCGuildMember?> GetGuildMemberByDiscordUserId(string discordUserId)
    {
        return await _context.FCMembers
            .Include(fcg => fcg.DiscordNameHistories)
            .Include(fcg => fcg.LodestoneNameHistories)
            .FirstOrDefaultAsync(fcg => fcg.DiscordUserUId == discordUserId);
    }

}