namespace Darjeeling.Models;

public class LodestoneWebResult
{
    public bool Success { get; set; }
    public string? ResultValue { get; set; }
    
    public LodestoneWebResult(bool isSuccess, string? resultValue = null)
    {
        Success = isSuccess;
        ResultValue = resultValue;
    }
    
    public static LodestoneWebResult AsSuccess(string? resultValue = null)
    {
        return new LodestoneWebResult(true);
    }
    
    public static LodestoneWebResult AsFailure(string errorMessage)
    {
        return new LodestoneWebResult(false, errorMessage);
    }
}