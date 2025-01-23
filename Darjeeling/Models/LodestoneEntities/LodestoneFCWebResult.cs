namespace Darjeeling.Models.LodestoneEntities;

public class LodestoneFCWebResult
{
    public bool Success { get; set; }
    public string? FreeCompanyName { get; set; }
    
    public ICollection<LodestoneFCMember>? Members { get; set; }

    public string? ConcatenatedMembers
    {
        get
        {
            if (Members != null && Members.Any())
            {
                return string.Join(", ", Members.Select(m => m.FullName));
            }
            return null;
        }
    }
    
    public LodestoneFCWebResult(bool isSuccess, string? freeCompanyName = null, List<LodestoneFCMember>? members = null)
    {
        Success = isSuccess;
        FreeCompanyName = freeCompanyName;
        Members = members;
    }

    
    public static LodestoneFCWebResult AsSuccess(string? freeCompanyName = null, List<LodestoneFCMember>? members = null)
    {
        return new LodestoneFCWebResult(true, freeCompanyName, members);
    }
    
    public static LodestoneFCWebResult AsFailure(string errorMessage)
    {
        return new LodestoneFCWebResult(false, errorMessage, null);
    }
}