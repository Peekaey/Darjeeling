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
    
}