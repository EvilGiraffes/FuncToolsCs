namespace FuncToolsCs;
public static class EitherExt
{
    public static Maybe<TLeft> Left<TLeft, TRight>(this Either<TLeft, TRight> either)
        where TLeft : notnull
        => either.Match(
            left => Maybe.Some(left),
            _ => Maybe.None<TLeft>());
    public static Maybe<TRight> Right<TLeft, TRight>(this Either<TLeft, TRight> either)
        where TRight : notnull
        => either.Match(
            _ => Maybe.None<TRight>(),
            right => Maybe.Some(right));
    public static Either<TRight, TLeft> Flip<TLeft, TRight>(this Either<TLeft, TRight> either)
        => either.Match(
            left => Either<TRight, TLeft>.Right(left),
            right => Either<TRight, TLeft>.Left(right));
    public static Either<TReturn, TRight> FlattenLeft<TRight, TReturn, TOtherLeft, TOtherRight>(
        this Either<Either<TOtherLeft, TOtherRight>, TRight> either,
        Func<TOtherLeft, TReturn> onLeft,
        Func<TOtherRight, TReturn> onRight)
        => either.MapLeft(inner => inner.Match(onLeft, onRight));
    public static Either<TLeft, TReturn> FlattenRight<TLeft, TReturn, TOtherLeft, TOtherRight>(
        this Either<TLeft, Either<TOtherLeft, TOtherRight>> either,
        Func<TOtherLeft, TReturn> onLeft,
        Func<TOtherRight, TReturn> onRight)
        => either.MapRight(inner => inner.Match(onLeft, onRight));
}
