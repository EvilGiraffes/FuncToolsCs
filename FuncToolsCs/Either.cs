using System.Diagnostics.CodeAnalysis;

namespace FuncToolsCs;
// TESTME: 
public readonly struct Either<TLeft, TRight>
{
    [MemberNotNullWhen(true, nameof(left))]
    [MemberNotNullWhen(false, nameof(right))]
    public bool IsLeft { get; }
    [MemberNotNullWhen(false, nameof(left))]
    [MemberNotNullWhen(true, nameof(right))]
    public bool IsRight
        => !IsLeft;
    readonly TLeft? left;
    readonly TRight? right;
    Either(TLeft left) : this()
    {
        this.left = left;
        IsLeft = true;
    }
    Either(TRight right) : this()
    {
        this.right = right;
        IsLeft = false;
    }
    public Either<TReturnLeft, TReturnRight> Map<TReturnLeft, TReturnRight>(
        Func<TLeft, TReturnLeft> onLeft,
        Func<TRight, TReturnRight> onRight)
        => IsLeft
        ? onLeft(left)
        : onRight(right);
    public Either<TReturn, TRight> MapLeft<TReturn>(Func<TLeft, TReturn> onLeft)
        => IsLeft
        ? onLeft(left)
        : right;
    public Either<TLeft, TReturn> MapRight<TReturn>(Func<TRight, TReturn> onRight)
        => IsLeft
        ? left
        : onRight(right);
    public Either<TReturnLeft, TReturnRight> Bind<TReturnLeft, TReturnRight>(
        Func<TLeft, Either<TReturnLeft, TReturnRight>> onLeft,
        Func<TRight, Either<TReturnLeft, TReturnRight>> onRight)
        => IsLeft
        ? onLeft(left)
        : onRight(right);
    public Either<TReturn, TRight> BindLeft<TReturn>(Func<TLeft, Either<TReturn, TRight>> onLeft)
        => IsLeft
        ? onLeft(left)
        : right;
    public Either<TLeft, TReturn> BindRight<TReturn>(Func<TRight, Either<TLeft, TReturn>> onRight)
        => IsLeft
        ? left
        : onRight(right);
    public Either<TLeft, TRight> InspectLeft(Action<TLeft> onLeft)
    {
        if (IsLeft)
            onLeft(left);
        return this;
    }
    public Either<TLeft, TRight> InspectRight(Action<TRight> onRight)
    {
        if (IsRight)
            onRight(right);
        return this;
    }
    public TReturn Match<TReturn>(Func<TLeft, TReturn> onLeft, Func<TRight, TReturn> onRight)
        => IsLeft
        ? onLeft(left)
        : onRight(right);
    public static Either<TLeft, TRight> Left(TLeft value)
        => new(value);
    public static Either<TLeft, TRight> Right(TRight value)
        => new(value);
    public static implicit operator Either<TLeft, TRight>(TLeft value)
        => new(value);
    public static implicit operator Either<TLeft, TRight>(TRight value)
        => new(value);
}
