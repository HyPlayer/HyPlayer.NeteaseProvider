namespace HyPlayer.NeteaseProvider.Bases;

public class ExceptionedErrorBase : ErrorResultBase
{

    public Exception? Exception { get; }
    
    public ExceptionedErrorBase(int errorCode, string? errorMessage = null, Exception? exception = null) : base(errorCode, errorMessage)
    {
        Exception = exception;
    }
}