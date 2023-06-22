namespace HyPlayer.NeteaseProvider.Extensions;

public readonly struct Results<TValue,TError>
{
    private readonly TValue? _value;
    private readonly TError? _error;

    public bool IsError { get; }
    public bool IsSuccess => !IsError;
    
    private Results(TValue value)
    {
        IsError = false;
        _value = value;
    }
    
    private Results(TError error)
    {
        IsError = true;
        _error = error;
    }

    public static implicit operator Results<TValue, TError>(TValue value) => new(value);
    public static implicit operator Results<TValue, TError>(TError error) => new(error);

    public static Results<TValue, TError> CreateError(TError error) => new(error);
    public static Results<TValue, TError> CreateSuccess(TValue value) => new(value);
    
    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> error)
        => IsSuccess ? success(_value!) : error(_error!);

}