using Darjeeling.Models.Entities;

namespace Darjeeling.Interfaces.Repositories;

public interface IFCGuildMemberRepository
{
    Task AddRangeAsync(List<FCGuildMember> member);
    Task RemoveRangeAsync(List<FCGuildMember> member);
    Task<List<FCGuildMember>> GetGuildMembersByGuildId(string guildId);
    Task UpdateRangeAsync(List<FCGuildMember> member);
}