namespace HyPlayer.NeteaseProvider.Bases;

public class ErrorResultBase : Exception
{
    public ErrorResultBase(int errorCode, string? errorMessage = null): base(errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public int ErrorCode { get; set; }
    public string? ErrorMessage { get; set; }
}