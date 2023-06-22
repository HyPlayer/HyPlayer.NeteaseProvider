namespace HyPlayer.NeteaseProvider.Bases;

public class ErrorResultBase
{
    public ErrorResultBase(int errorCode, string? errorMessage = null)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public int ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}