using HyPlayer.NeteaseApi.Bases;
using HyPlayer.NeteaseApi.Extensions;
using HyPlayer.PlayCore.Abstraction.Models;

namespace HyPlayer.NeteaseProvider.Extensions;

public static class ResultMappingExtensions
{
    public static PlayCoreResult<TValue> ToPlayCoreResult<TValue>(this Results<TValue, ErrorResultBase> result)
    {
        return result.Match(
            PlayCoreResult<TValue>.CreateSuccess,
            error => PlayCoreResult<TValue>.CreateError(
                error.ErrorCode.ToString(),
                error.Message ?? string.Empty,
                error is ExceptionedErrorBase exceptioned ? exceptioned.InnerException ?? error : error));
    }

    public static PlayCoreResult<TResult> ToPlayCoreResult<TValue, TResult>(
        this Results<TValue, ErrorResultBase> result,
        Func<TValue, TResult> successMapper)
    {
        return result.Match(
            success => PlayCoreResult<TResult>.CreateSuccess(successMapper(success)),
            error => PlayCoreResult<TResult>.CreateError(
                error.ErrorCode.ToString(),
                error.Message ?? string.Empty,
                error is ExceptionedErrorBase exceptioned ? exceptioned.InnerException ?? error : error));
    }
}
