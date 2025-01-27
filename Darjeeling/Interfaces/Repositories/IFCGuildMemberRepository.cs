using Darjeeling.Models.Entities;

namespace Darjeeling.Interfaces.Repositories;

public interface IFCGuildMemberRepository
{
    Task AddRangeAsync(List<FCGuildMember> member);
    Task RemoveRangeAsync(List<FCGuildMember> member);
}