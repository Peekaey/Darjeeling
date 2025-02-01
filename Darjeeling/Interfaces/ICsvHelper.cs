using Darjeeling.Models.DTOs;

namespace Darjeeling.Interfaces;

public interface ICsvHelper
{
    Task<MemoryStream> CreateTableCsv<T>(List<T> records);
}