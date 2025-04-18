﻿namespace HyPlayer.NeteaseApi.Extensions;

public struct Results<TValue, TError>
{
    private TValue? _value;
    private TError? _error;

    public bool IsError => !IsSuccess;
    public bool IsSuccess { get; set; }

    public TValue? Value => _value;
    public TError? Error => _error;

    private Results(TValue value)
    {
        IsSuccess = true;
        _value = value;
    }

    private Results(TError error)
    {
        IsSuccess = false;
        _error = error;
    }

    public static implicit operator Results<TValue, TError>(TValue value) => new(value);
    public static implicit operator Results<TValue, TError>(TError error) => new(error);

    public static Results<TValue, TError> CreateError(TError error) => new(error);
    public static Results<TValue, TError> CreateSuccess(TValue value) => new(value);

    public Results<TValue, TError> WithValue(TValue value)
    {
        _value = value;
        return this;
    }

    public Results<TValue, TError> WithError(TError error)
    {
        _error = error;
        return this;
    }

    public TResult Match<TResult>(Func<TValue, TResult> success, Func<TError, TResult> error)
        => IsSuccess ? success(_value!) : error(_error!);

}