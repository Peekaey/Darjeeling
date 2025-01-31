using Darjeeling.Models.DTOs;

namespace Darjeeling.Interfaces;

public interface ICsvHelper
{
    Task<MemoryStream> CreateMemberListCsv(List<FCGuildMemberDTO> fcGuildMembers);
}