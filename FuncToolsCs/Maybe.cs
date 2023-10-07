using System.Diagnostics.CodeAnalysis;

namespace FuncToolsCs;
public static class Maybe
{
    public static Maybe<T> Some<T>(T value)
        where T : notnull
        => Maybe<T>.Some(value);
    public static Maybe<T> None<T>()
        where T : notnull
        => Maybe<T>.None();
}
public readonly struct Maybe<T>
    where T : notnull
{
    [MemberNotNullWhen(false, nameof(value))]
    public bool IsNone { get; }
    [MemberNotNullWhen(true, nameof(value))]
    public bool IsSome
        => !IsNone;
    readonly T? value;
    Maybe(T? value, bool isEmpty)
    {
        this.value = value;
        IsNone = isEmpty;
    }
    public Maybe<TReturn> Map<TReturn>(Func<T, TReturn> map)
        where TReturn : notnull
        => IsNone
        ? Maybe<TReturn>.None()
        : Maybe<TReturn>.Some(map(value));
    public Maybe<TReturn> Bind<TReturn>(Func<T, Maybe<TReturn>> bind)
        where TReturn : notnull
        => IsNone
        ? Maybe<TReturn>.None()
        : bind(value);
    public Maybe<T> Filter(Predicate<T> filter)
    {
        if (IsNone)
            return None();
        if (!filter(value))
            return None();
        return this;
    }
    public TReturn Match<TReturn>(Func<T, TReturn> onSome, TReturn onNone)
        => IsNone
        ? onNone
        : onSome(value);
    public TReturn Match<TReturn>(Func<T, TReturn> onSome, Func<TReturn> onNone)
        => IsNone
        ? onNone()
        : onSome(value);
    public T? Unwrap()
        => IsNone
        ? default
        : value;
    public Result<T, TError> UnwrapResult<TError>(Func<TError> exceptionFactory)
        where TError : Exception
    {
        if (IsSome)
            return value;
        return exceptionFactory();
    }
    public T UnwrapOrThrow<TError>(Func<TError> exceptionFactory)
        where TError : Exception
    {
        if (IsSome)
            return value;
        throw exceptionFactory();
    }
    public T UnwrapOr(T defaultValue)
        => IsNone
        ? defaultValue
        : value;
    public T UnwrapOr(Func<T> defaultValueFactory)
        => IsNone
        ? defaultValueFactory()
        : value;
    public static Maybe<T> Some(T value)
        => new(value, false);
    public static Maybe<T> None()
        => new(default, true);
}
