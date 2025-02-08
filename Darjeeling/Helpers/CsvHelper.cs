using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Darjeeling.Interfaces;
using Darjeeling.Models.DTOs;

namespace Darjeeling.Helpers;

public class CsvHelper : ICsvHelper
{
    
    public async Task<MemoryStream> CreateTableCsv<T>(List<T> records)
    {
        var memoryStream = new MemoryStream();

        await using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
        {
            await using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                await csv.WriteRecordsAsync(records);
                await writer.FlushAsync();
                memoryStream.Position = 0;
            }
        }
        
        return memoryStream;
        
    }
    public async Task<MemoryStream> CreateGuildMemberNameHistoryDtoCsv(
        GuildMemberNameHistoryDTO guildMemberNameHistoryDto)
    {
        var memoryStream = new MemoryStream();
    
        await using (var writer = new StreamWriter(memoryStream, leaveOpen: true))
        await using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            // Write DiscordNameHistories header and records
            csv.WriteHeader<DiscordNameHistoryDTO>();
            await csv.NextRecordAsync(); // Ends the header row
            await csv.WriteRecordsAsync(guildMemberNameHistoryDto.DiscordNameHistories);
        
            // Write a blank line as a buffer between groups
            await writer.WriteLineAsync();
        
            // Write LodestoneNameHistories header and records
            csv.WriteHeader<LodestoneNameHistoryDTO>();
            await csv.NextRecordAsync(); // Ends the header row
            await csv.WriteRecordsAsync(guildMemberNameHistoryDto.LodestoneNameHistories);
        }
    
        // Reset the stream position before returning so it can be read from the beginning
        memoryStream.Position = 0;
        return memoryStream;
    }

}