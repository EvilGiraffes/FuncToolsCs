using FluentAssertions;
using FluentAssertions.Execution;

namespace FuncToolsCs.FluentAssertions;
public sealed class MaybeAssertions<T>
    where T : notnull
{
    public Maybe<T> Subject { get; }
    AndConstraint<MaybeAssertions<T>> AndConstraint
        => new(this);
    public MaybeAssertions(Maybe<T> subject)
    {
        Subject = subject;
    }
    public AndConstraint<MaybeAssertions<T>> BeSome(string? because = null, params object?[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsSome)
            .FailWith("Expected result not to be an error{reason}.");
        return AndConstraint;
    }
    public AndConstraint<MaybeAssertions<T>> BeNone(string? because = null, params object?[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.IsNone)
            .FailWith("Expected result to be an error{reason}.");
        return AndConstraint;
    }
    public AndConstraint<MaybeAssertions<T>> Contain(T value, string? because = null, params object?[] becauseArgs)
    {
        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(Subject.Match(
                inner => inner?.Equals(value) ?? false,
                () => false))
            .FailWith("Expected result to match {0}{reason}.", value);
        return AndConstraint;
    }
}

public static class MaybeAssertionAttachment
{
    public static MaybeAssertions<T> Should<T>(this Maybe<T> maybe)
        where T : notnull
        => new(maybe);
}
