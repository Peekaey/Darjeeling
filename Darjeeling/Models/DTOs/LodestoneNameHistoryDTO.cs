namespace Darjeeling.Models.DTOs;

public class LodestoneNameHistoryDTO
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public DateTime DateAdded { get; set; }

}