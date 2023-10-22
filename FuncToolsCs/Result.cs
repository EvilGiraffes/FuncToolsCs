using System.Diagnostics.CodeAnalysis;

using FuncToolsCs.Errors;

namespace FuncToolsCs;
public static class UnitResult
{
    public static Result<Unit, TError> Ok<TError>()
        => Unit.Instance;
    public static Result<Unit, TError> Error<TError>(TError error)
        => error;
}
public readonly struct Result<T, TError>
{
    [MemberNotNullWhen(true, nameof(error))]
    [MemberNotNullWhen(false, nameof(value))]
    public bool IsError { get; }
    [MemberNotNullWhen(false, nameof(error))]
    [MemberNotNullWhen(true, nameof(value))]
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
        => IsError
        ? new Result<TReturn, TReturnError>(onError(error))
        : binder(value);
    public Result<T, TError> Handle(Action<TError> onError)
    {
        if (IsError)
            onError(error);
        return this;
    }
    public Result<T, TError> Inspect(Action<T> onOk)
    {
        if (IsOk)
            onOk(value);
        return this;
    }
    public TReturn Match<TReturn>(Func<T, TReturn> onOk, Func<TError, TReturn> onError)
        => IsError
        ? onError(error)
        : onOk(value);
    public T Unwrap()
    {
        if (IsOk)
            return value;
        if (error is Exception exc)
            throw exc;
        throw new ResultIsError<TError>()
        {
            Error = error,
        };
    }
    public bool TryUnwrap([NotNullWhen(true)] out T? result)
    {
        if (IsError)
        {
            result = default;
            return false;
        }
        result = value;
        return true;
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
