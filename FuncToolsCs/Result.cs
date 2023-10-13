using System.Diagnostics.CodeAnalysis;

namespace FuncToolsCs;
public readonly struct Result<T, TError>
    where TError : Exception
{
    [MemberNotNullWhen(true, nameof(error))]
    [MemberNotNullWhen(false, nameof(value))]
    public bool IsError { get; }
    public bool IsOk
        => !IsError;
    readonly T? value;
    readonly TError? error;
    Result(T value) : this()
    {
        this.value = value;
        IsError = false;
    }
    Result(TError error) : this()
    {
        this.error = error;
        IsError = true;
    }
    public Result<TReturn, TError> Map<TReturn>(Func<T, TReturn> map)
        => IsError
        ? new Result<TReturn, TError>(error)
        : new Result<TReturn, TError>(map(value));
    public Result<T, TReturnError> MapError<TReturnError>(Func<TError, TReturnError> map)
        where TReturnError : Exception
        => IsError
        ? new Result<T, TReturnError>(map(error))
        : new Result<T, TReturnError>(value);
    public Result<TReturn, TError> Bind<TReturn>(Func<T, Result<TReturn, TError>> binder)
        => IsError
        ? new Result<TReturn, TError>(error)
        : binder(value);
    public Result<TReturn, TReturnError> BindOrMapError<TReturn, TReturnError>(
        Func<T, Result<TReturn, TReturnError>> binder,
        Func<TError, TReturnError> onError)
        where TReturnError : Exception
        => IsError
        ? new Result<TReturn, TReturnError>(onError(error))
        : binder(value);
    public TReturn Match<TReturn>(Func<T, TReturn> onOk, Func<TError, TReturn> onError)
        => IsError
        ? onError(error)
        : onOk(value);
    public T Unwrap()
    {
        if (IsError)
            throw error;
        return value;
    }
    public T UnwrapOrThrow<TMappedError>(Func<TError, TMappedError> onError)
        where TMappedError : Exception
    {
        if (IsError)
            throw onError(error);
        return value;
    }
    public T UnwrapOr(T defaultValue)
        => IsError
        ? defaultValue
        : value;
    public T UnwrapOr(Func<T> defaultValue)
        => IsError
        ? defaultValue()
        : value;
    public static Result<T, TError> Ok(T value)
        => new(value);
    public static Result<T, TError> Error(TError error)
        => new(error);
    public static implicit operator Result<T, TError>(T value)
        => new(value);
    public static implicit operator Result<T, TError>(TError error)
        => new(error);

}
