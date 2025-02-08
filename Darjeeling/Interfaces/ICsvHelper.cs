using Darjeeling.Models.DTOs;

namespace Darjeeling.Interfaces;

public interface ICsvHelper
{
    Task<MemoryStream> CreateTableCsv<T>(List<T> records);

    Task<MemoryStream> CreateGuildMemberNameHistoryDtoCsv(GuildMemberNameHistoryDTO guildMemberNameHistoryDto);
}