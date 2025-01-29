using Darjeeling.Models.Entities;

namespace Darjeeling.Models;

public class UpdatedFCMembersResult
{
    public List<FCGuildMember> UpdatedFCGuildMembers { get; set; }
    public List<FCGuildMember> NewFCGuildMembers { get; set; }
}