namespace Darjeeling.Models;

public class RegisterServiceResult
{
    public bool Success { get; set; }
    public int RegisteredMemberCount { get; set; }
    public string ErrorMessage { get; set; }
    
    
    public RegisterServiceResult(bool success, int registeredMemberCount)
    {
        Success = success;
        RegisteredMemberCount = registeredMemberCount;
    }
    
    public static RegisterServiceResult AsSuccess(int registeredMemberCount)
    {
        return new RegisterServiceResult(true, registeredMemberCount);
    }
    
    public static RegisterServiceResult AsFailure(string errorMessage)
    {
        return new RegisterServiceResult(false, 0)
        {
            ErrorMessage = errorMessage
        };
    }
    
    
    
}