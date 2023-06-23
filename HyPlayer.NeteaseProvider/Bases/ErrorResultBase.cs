namespace HyPlayer.NeteaseProvider.Bases;

public class ErrorResultBase : Exception
{
    public ErrorResultBase(int errorCode, string? errorMessage = null): base(errorMessage)
    {
        ErrorCode = errorCode;
    }

    public int ErrorCode { get; set; }
}