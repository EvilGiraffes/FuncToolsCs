using System.Diagnostics.CodeAnalysis;

using FuncToolsCs.Errors;

namespace FuncToolsCs;
public static class Maybe
{
    public static Maybe<T> Some<T>(T value)
        where T : notnull
        => Maybe<T>.Some(value);
    public static Maybe<T> None<T>()
        where T : notnull
        => Maybe<T>.None();
    public static Maybe<T> From<T>(T? value)
        where T : class
        => value is null
        ? None<T>()
        : Some(value);
    public static Maybe<T> Take<T>(ref Maybe<T> from)
        where T : notnull
        => Maybe<T>.Take(ref from);
    public static Maybe<T> TakeIf<T>(ref Maybe<T> from, Func<T, bool> filter)
        where T : notnull
        => Maybe<T>.TakeIf(ref from, filter);
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
    Maybe(T? value, bool isEmpty) : this()
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
    public Maybe<T> Filter(Func<T, bool> filter)
    {
        if (IsNone)
            return None();
        if (!filter(value))
            return None();
        return this;
    }
    public Maybe<(T, TOther)> Zip<TOther>(Maybe<TOther> other)
        where TOther : notnull
    {
        if (IsNone || other.IsNone)
            return Maybe<(T, TOther)>.None();
        return Maybe<(T, TOther)>.Some((value, other.value));
    }
    public TReturn Match<TReturn>(Func<T, TReturn> onSome, TReturn onNone)
        => IsNone
        ? onNone
        : onSome(value);
    public TReturn Match<TReturn>(Func<T, TReturn> onSome, Func<TReturn> onNone)
        => IsNone
        ? onNone()
        : onSome(value);
    public T Expect(Maybe<string> reason)
    {
        if (IsSome)
            return value;
        string format = $"Expected the maybe to have a value of type '{typeof(T).Name}' contained{{0}}";
        string fullMessage = reason.Match(
            inner => string.Format(format, $" because: {inner}."),
            () => string.Format(format, '.'));
        throw new MaybeIsNone(fullMessage);
    }
    public T Expect()
        => Expect(Maybe<string>.None());
    public T Expect(string reason)
        => Expect(Maybe<string>.Some(reason));
    public T? Unwrap()
        => IsNone
        ? default
        : value;
    public bool TryUnwrap([NotNullWhen(true)] out T? value)
    {
        if (IsNone)
        {
            value = default;
            return false;
        }
        value = this.value;
        return true;
    }
    public Result<T, MaybeIsNone> UnwrapResult()
    {
        if (IsSome)
            return value;
        return new MaybeIsNone();
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
    internal static Maybe<T> Take(ref Maybe<T> from)
    {
        if (!from.TryUnwrap(out T? value))
            return from;
        from = Maybe<T>.None();
        return Maybe<T>.Some(
            value);
    }
    internal static Maybe<T> TakeIf(ref Maybe<T> from, Func<T, bool> filter)
    {
        if (!from.TryUnwrap(out T? value))
            return from;
        if (!filter(value))
            return Maybe<T>.None();
        from = Maybe<T>.None();
        return Maybe<T>.Some(value);
    }
    internal static Maybe<T> Some(T value)
        => new(value, false);
    internal static Maybe<T> None()
        => new(default, true);
}
