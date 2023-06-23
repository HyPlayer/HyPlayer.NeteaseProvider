namespace HyPlayer.NeteaseProvider.Bases;

public class ExceptionedErrorBase : ErrorResultBase
{
    public Exception? InnerException { get; }
    
    public ExceptionedErrorBase(int errorCode, string? errorMessage = null, Exception? exception = null) : base(errorCode, errorMessage)
    {
        InnerException = exception;
    }
}